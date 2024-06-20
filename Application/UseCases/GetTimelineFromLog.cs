using Application.DTO;
using Application.DTO.Enums;
using CacheService.Repositories;
using DataAccess.Entities;
using DataAccess.Enums;
using MapsterMapper;
using Shared;
using WarcraftLogsService.Models;

using Difficulty = DataAccess.Enums.Difficulty;
using EventType = WarcraftLogsService.Models.EventType;
using WoWSpecRole = DataAccess.Enums.WoWSpecRole;

namespace Application.UseCases;

public class GetTimelineFromLog
{
  private readonly BossCacheRepository _bossRepository;
  private readonly FightCacheRepository _fightRepository;
  private readonly AbilitiesCacheRepository _abilitiesRepository;
  private readonly StagesCacheRepository _stagesRepository;
  private readonly SpecsCacheRepository _specsRepository;
  private readonly IMapper _mapper;
  public GetTimelineFromLog(BossCacheRepository bossRepository, FightCacheRepository fightRepository,
    AbilitiesCacheRepository abilitiesRepository, StagesCacheRepository stagesRepository, SpecsCacheRepository specsRepository,
    IMapper mapper)
    => (_bossRepository, _fightRepository, _abilitiesRepository, _stagesRepository, _specsRepository, _mapper) = 
      (bossRepository, fightRepository, abilitiesRepository, stagesRepository, specsRepository, mapper);
  
  public async Task<TimelineFromLogDto?> Execute(string code, int? sourceId = null)
  {
    var fight = await GetLastFightFromLog(code, sourceId);
    if (fight == null) return null;
    
    return GetTimeline(fight);
  }
  
  public async Task<TimelineFromLogDto?> Execute(string code, uint fightId, int? sourceId = null)
  {
    var fight = await GetFightFromLog(code, fightId, sourceId);
    if (fight == null) return null;

    return GetTimeline(fight);
  }

  private TimelineFromLogDto? GetTimeline(Fight fight)
  {
    var boss = _bossRepository.GetFullBoss(fight.BossId, fight.Difficulty.Map<Difficulty>());
    if (boss == null) return null;
    MoveBossTimers(boss, fight.BossAbilities, fight.Duration);
    
    var players = CreatePlayersAndBindStages(boss.Zone.ExpansionId ?? "clwc3su3f000008mmdq5d19a3", fight.Players, boss.BossStages);
    
    var result = new TimelineFromLogDto()
    {
      Boss = _mapper.Map<FullBossDto>(boss),
      Players = players
    };
    return result;
  }
  
  private async Task<Fight?> GetLastFightFromLog(string code, int? sourceId = null)
  {
    var playerAbilitiesIds = _abilitiesRepository.GetAllAbilitiesIds();
    var bossStagesIds = _stagesRepository.GetStagesActivationIds();
    return await _fightRepository.GetLastFight(code, bossStagesIds, playerAbilitiesIds, sourceId);
  }
  
  private async Task<Fight?> GetFightFromLog(string code, uint fightId, int? sourceId = null)
  {
    var playerAbilitiesIds = _abilitiesRepository.GetAllAbilitiesIds();
    var bossStagesIds = _stagesRepository.GetStagesActivationIds();
    return await _fightRepository.GetFight(code, fightId, bossStagesIds, playerAbilitiesIds, sourceId);
  }

  private void MoveBossTimers(Boss boss, ICollection<FightAbility> fightAbilities, int fightDuration)
  {
    foreach (var currentStage in boss.BossStages.OrderBy(x => x.StageNumber))
    {
      if (currentStage.EventType == null || currentStage.EventCount == null || currentStage.AbilityId == null) continue;
      
      var activationAbility = fightAbilities.FirstOrDefault(x => x.Id == currentStage.AbilityId);
      if (activationAbility == null) continue;
      
      var activationEvent = activationAbility.GetActivationEvent(currentStage.EventType.Map<EventType>(),
        currentStage.EventCount.Value);
      if (activationEvent == null) continue;

      var prevStage = boss.BossStages.FirstOrDefault(x => x.StageNumber == currentStage.StageNumber - 1);
      if (prevStage == null) continue;

      var diffTime = activationEvent.AbsoluteTimer - prevStage.EndTimer;
      MoveStageEndTimer(boss, prevStage, diffTime);
      MoveAbilities(boss, currentStage, diffTime);
    }

    boss.FightDuration = fightDuration;
    var lastStage = boss.BossStages.MaxBy(x => x.StageNumber)!;
    MoveStageEndTimer(boss, lastStage, fightDuration - lastStage.EndTimer);
  }

  private void MoveStageEndTimer(Boss boss, BossStage stageToMove, int diffTime)
  {
    if (boss.FightType == FightType.StageIndependantTimers)
    {
      stageToMove.EndTimer += diffTime;
      stageToMove.MinTimer += diffTime;
      stageToMove.MaxTimer -= diffTime;
    }

    if (boss.FightType == FightType.StageDependantTimers)
    {
      foreach (var stage in boss.BossStages)
      {
        if (stage.StageNumber == stageToMove.StageNumber)
        {
          stage.EndTimer += diffTime;
          stage.MinTimer += diffTime;
          stage.MaxTimer -= diffTime;
        }
        if (stage.StageNumber > stageToMove.StageNumber)
        {
          stage.EndTimer += diffTime;
          stage.StartTimer += diffTime;
        }
      }
    }
  }

  private void MoveAbilities(Boss boss, BossStage? nextStage, int diffTime)
  {
    if (boss.FightType == FightType.StageIndependantTimers) return;

    foreach (var ability in boss.BossAbilities)
    {
      var isActivationAbility = nextStage != null && ability.InGameId == nextStage.AbilityId;

      foreach (var @event in ability.BossEvents)
      {
        var stage = boss.BossStages.FirstOrDefault(x => x.Id == @event.StageId);
        if (stage == null) continue;

        var isNeedToChangeAbsoluteTimer = @event.RelativeTimer != null;
        if (isNeedToChangeAbsoluteTimer)
        {
          @event.AbsoluteTimer = stage.StartTimer + @event.RelativeTimer!.Value;
        }
        
        var isNeedToChangeDurationFn = () =>
        {
          if (@event.Duration == 0) return false;
          if (!isActivationAbility) return false;
          if (nextStage == null) return false;

          if (@event.EventType != DataAccess.Enums.EventType.Cast ||
              @event.EventType != DataAccess.Enums.EventType.RemoveBuff ||
              @event.EventType != DataAccess.Enums.EventType.RemoveDebuff)
            return false;

          return @event.AbilityCount == nextStage.EventCount && @event.EventType == nextStage.EventType;
        };
        var isNeedToChangeDuration = isNeedToChangeDurationFn();

        if (isNeedToChangeDuration)
        {
          @event.Duration += diffTime;
        }
        
      }
    }
  }
  
  private Dictionary<string, PlayerDto> CreatePlayersAndBindStages(string expansionId, ICollection<FightPlayer> players, ICollection<BossStage> stages)
  {
    var specs = _specsRepository.GetSpecsWithAbilities(expansionId);
    var result = new Dictionary<string, PlayerDto>();
    
    foreach (var player in players)
    {
      var playerSpec = SearchPlayerSpec(player, specs);
      if (playerSpec == null) continue;

      var newPlayer = new PlayerDto()
      {
        SpecId = playerSpec.Id,
        Role = playerSpec.Role.Map<DTO.Enums.WoWSpecRole>(),
        Color = playerSpec.Color,
        Name = player.Name, 
        Events = new List<PlayerEventDto>()
      };
      
      foreach (var ability in player.Abilities)
      {
        var dbAbility = playerSpec.Abilities.FirstOrDefault(x => x.Id == ability.Id.ToString() ||
                                                                 x.ExtraIds.Contains(ability.Id.ToString()));
        if (dbAbility is null) continue;
        
        foreach (var @event in ability.Events)
        {
          if (@event.Type != EventType.Cast) continue;
          
          var stage = stages.FirstOrDefault(x => x.StartTimer < @event.AbsoluteTimer && 
                                                 x.EndTimer >= @event.AbsoluteTimer) ?? stages.First();

          var newEvent = new PlayerEventDto()
          {
            AbsoluteTimer = @event.AbsoluteTimer,
            RelativeTimer = @event.AbsoluteTimer - stage.StartTimer,
            AbilityId = dbAbility.Id,
            Options = new PlayerEventOptionsDto()
            {
              EventAbilityId = stage.AbilityId?.ToString(),
              EventCount = stage.EventCount,
              EventTypeForNote = stage.EventTypeForNote?.Map<EventTypeForNoteDto>()
            }
          };
          newPlayer.Events.Add(newEvent);
        }
      }
      if (newPlayer.Events.Count != 0) result.Add(Guid.NewGuid().ToString(), newPlayer);
    }

    return result;
  }
  
  private WoWSpec? SearchPlayerSpec(FightPlayer player, ICollection<WoWSpec> specs)
  {
    var possibleSpecs = specs.Where(x => x.InGameClass == player.Class).ToList();
    if (possibleSpecs.Count == 1) return possibleSpecs.First();

    possibleSpecs = possibleSpecs.Where(x => x.Name == player.Spec).ToList();
    if (possibleSpecs.Count == 1) return possibleSpecs.First();

    return specs.FirstOrDefault(x => x.InGameClass == player.Class &&
                                     (x.Role is WoWSpecRole.Melee or WoWSpecRole.Ranged));
  }
  
}