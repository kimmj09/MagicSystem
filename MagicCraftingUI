using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicCraftingUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject craftingUIRoot;      // MagicCraftingPanel
    public Transform elementTabContainer;  // LeftTabContainer
    public Transform cardListContainer;    // 카드 버튼들이 나열될 부모 패널

    [Header("UI Prefabs")]
    public GameObject cardButtonPrefab;    // 카드 모양 UI 버튼 프리팹

    [Header("Magic Databases")]
    public List<ElementCardSO> allElementCards;           // 보유한 모든 원소 카드 목록
    public List<TransformCardSO> commonTransformCards;    // 공통 변형 카드 (라인, 폭발 등)
    
    [Header("Exclusive Transform Cards")]
    public List<TransformCardSO> atmosphereTransformCards;// 대기 전용 변형 카드
    public List<TransformCardSO> lightTransformCards;     // 빛 전용 변형 카드 (추후 사용)

    [Header("References")]
    public MagicCaster caster;

    public void OpenCraftingUI()
    {
        if (craftingUIRoot != null) craftingUIRoot.SetActive(true);
    }

    public void CloseCraftingUI()
    {
        if (craftingUIRoot != null) craftingUIRoot.SetActive(false);
        ClearCardList();
    }

    public void Start()
    {
        if (craftingUIRoot != null) craftingUIRoot.SetActive(false);
        ClearCardList();
    }

    // 탭 버튼의 OnClick() 이벤트에 연결할 함수 (인자: 0 = Atmosphere, 1 = Light)
    public void OnSelectElementTab(int elementTypeIndex)
    {
        ClearCardList();
        ElementType selectedType = (ElementType)elementTypeIndex;

        // 1. [맨 위] 선택한 속성에 맞는 원소 카드 생성
        ElementCardSO targetElement = allElementCards.Find(e => e.elementType == selectedType);
        if (targetElement != null)
        {
            CreateElementCardButton(targetElement);
        }

        // 2. [중간] 공통 변형 마법 카드 나열 (직선, 폭발 등)
        foreach (var transCard in commonTransformCards)
        {
            if (transCard != null) CreateTransformCardButton(transCard);
        }

        // 3. [끝] 원소 전용 변형 마법 카드 나열 (현재 비어있어도 에러 없이 넘어가며, 추후 등록 시 자동 노출)
        List<TransformCardSO> exclusiveCards = GetExclusiveCards(selectedType);
        if (exclusiveCards != null)
        {
            foreach (var transCard in exclusiveCards)
            {
                if (transCard != null) CreateTransformCardButton(transCard);
            }
        }
    }

    // 속성별 전용 변형 카드 리스트 반환
    private List<TransformCardSO> GetExclusiveCards(ElementType type)
    {
        return type switch
        {
            ElementType.atmosphere => atmosphereTransformCards,
            ElementType.Light => lightTransformCards,
            _ => null
        };
    }

    // 원소 카드 버튼 생성 및 클릭 이벤트 연결
    private void CreateElementCardButton(ElementCardSO elementSO)
    {
        GameObject btnObj = Instantiate(cardButtonPrefab, cardListContainer);
        btnObj.GetComponentInChildren<TextMeshProUGUI>().text = $"[원소] {elementSO.cardName}";
        
        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            if (caster != null) caster.currentElementCard = elementSO;
            Debug.Log($"원소 카드 선택됨: {elementSO.cardName}");
        });
    }

    // 변형 카드 버튼 생성 및 클릭 이벤트 연결
    private void CreateTransformCardButton(TransformCardSO transformSO)
    {
        GameObject btnObj = Instantiate(cardButtonPrefab, cardListContainer);
        btnObj.GetComponentInChildren<TextMeshProUGUI>().text = $"[변형] {transformSO.cardName}";

        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            if (caster != null) caster.currentTransformCard = transformSO;
            Debug.Log($"변형 카드 선택됨: {transformSO.cardName}");
        });
    }

    private void ClearCardList()
    {
        if (cardListContainer == null) return;
        foreach (Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
