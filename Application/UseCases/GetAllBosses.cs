using DataAccess.Entities;
using DataAccess.Repositories;

namespace Application.UseCases;

public class GetFullBoss
{
  private readonly BossRepository _bossRepository;

  public GetFullBoss(BossRepository bossRepository)
    => _bossRepository = bossRepository;

  public Boss? Handle(string id)
  {
    return _bossRepository.GetFullBoss(id);
  }
}