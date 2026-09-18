using UnityEngine;

public class MagicCaster : MonoBehaviour
{
    [Header("Card Slots")]
    public ElementCardSO currentElementCard;      // 연성판에 올라간 원소 카드
    public TransformCardSO currentTransformCard;  // 연성판에 올라간 변형 카드

    [Header("Player Pneuma (MP)")]
    public float currentPneuma = 100f;
    public float rawCastPenaltyCost = 50f;

    [Header("Time Dilation (Casting State)")]
    public bool isCasting = false;
    public float slowTimeFactor = 0.1f;
    public float logosStat = 1f;

    public MagicCraftingUI craftingUI;

    private void Update()
    {
        // F 키 입력 제어
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isCasting)
            {
                StartCasting();  // 캐스팅 상태가 아닐 때: 캐스팅 상태 진입
            }
            else
            {
                TryExecuteCraftedMagic();  // 캐스팅 상태일 때: 연성 확인 및 시전 (조합 미완성 시 자동 취소)
            }
        }

        // C 키 입력 제어: 캐스팅 취소
        if (isCasting && Input.GetKeyDown(KeyCode.C))
        {
            CancelCasting();
        }
    }

    // 캐스팅 진입 (시간 팽창 & UI 오픈)
    public void StartCasting()
    {
        isCasting = true;
        float currentFactor = Mathf.Clamp(slowTimeFactor / (1f + logosStat * 0.1f), 0.02f, 0.5f);
        
        Time.timeScale = currentFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (craftingUI != null) craftingUI.OpenCraftingUI();
        Debug.Log($"[캐스팅 진입] 시간이 {Time.timeScale * 100f}% 속도로 느려집니다.");
    }

    // F키 시전 시도 (예외 처리 포함)
    private void TryExecuteCraftedMagic()
    {
        // 예외 처리: 원소 카드가 없거나, 변형 카드만 세팅된 경우 캐스팅 취소
        if (currentElementCard == null)
        {
            Debug.Log("[연성 실패] 원소 카드가 선택되지 않아 캐스팅이 취소됩니다.");
            CancelCasting();
            return;
        }

        // 정상 조합 시전 (원소 + 변형)
        if (currentTransformCard != null)
        {
            float requiredCost = currentTransformCard.basePneumaCost;
            if (currentPneuma >= requiredCost)
            {
                currentPneuma -= requiredCost;
                currentTransformCard.ExecuteMagic(transform, currentElementCard);
                FinishCasting();
            }
            else
            {
                Debug.LogWarning("프뉴마(MP)가 부족합니다!");
            }
        }
        // 변형 카드가 없는 단일 원소 시전 예시 (필요시 조정 가능)
        else
        {
            Debug.Log($"[{currentElementCard.cardName}] 원소 단독 시전!");
            FinishCasting();
        }
    }

    // 캐스팅 취소 (C키 또는 예외 처리)
    public void CancelCasting()
    {
        ResetTimeScale();
        if (craftingUI != null) craftingUI.CloseCraftingUI();
        Debug.Log("[캐스팅 취소] 연성을 중단하고 복귀합니다.");
    }

    // 시전 완료 후 성공적으로 복귀
    private void FinishCasting()
    {
        ResetTimeScale();
        if (craftingUI != null) craftingUI.CloseCraftingUI();
        
        // 연성 슬롯 초기화
        currentElementCard = null;
        currentTransformCard = null;
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        isCasting = false;
    }

    private void OnDisable()
    {
        ResetTimeScale();
    }
}
