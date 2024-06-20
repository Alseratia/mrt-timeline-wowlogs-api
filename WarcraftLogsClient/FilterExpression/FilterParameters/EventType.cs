namespace WarcraftLogs;

public enum EventType
{
  begincast, cast, miss, damage, heal, absorbed,
  healabsorbed, applybuff, applydebuff, applybuffstack,
  applydebuffstack, refreshbuff, refreshdebuff, removebuff,
  removedebuff, removebuffstack, removedebuffstack, summon,
  create, death, destroy, extraattacks, aurabroken, dispel,
  interrupt, steal, leech, resourcechange, drain, resurrect,
  encounterstart, encounterend, dungeonstart, dungeonend,
  dungeonencounterstart, dungeonencounterend, towerstart,
  towerend, mapchange, zonechange, worldmarkerplaced,
  worldmarkerremoved, taunt, modifythreat, calculateddamage,
  calculatedheal
}
