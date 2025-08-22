public enum TargetType
{
    Self,           // 플레이어 자신 (ex: 버프, 실드, 회복 등)
    Enemy,          // 적 한 명
    AllEnemies,     // 적 전체
    Any,            // 적 중에서 하나 선택 (타겟팅)
    None            // 대상 필요 없음 (ex: 덱 효과 등)
}