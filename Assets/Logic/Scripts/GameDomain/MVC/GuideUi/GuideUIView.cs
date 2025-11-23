using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

public class GuideUIView : MonoBehaviour {
    public UIDocument uiDocument;

    [SerializeField] private string guideLabel = "Guides";

    private VisualElement root;
    private GuideSO currentGuide;
    private int currentPageIndex = 0;

    private Label titleLabel;
    private Label descriptionLabel;
    private VisualElement pageImageContainer;
    private VisualElement buttonsContainer;
    private VisualElement pagesButtonContainer;
    private VisualElement guidesListContainer;
    private Button nextPageButton;
    private Button previousPageButton;

    private readonly List<Button> pageButtons = new List<Button>();
    private readonly List<Button> guideButtons = new List<Button>();


    private async void InitiPoint() {
        root = uiDocument.rootVisualElement;

        titleLabel = root.Q<Label>("title-label");
        descriptionLabel = root.Q<Label>("description-label");
        pageImageContainer = root.Q<VisualElement>("page-image-container");
        buttonsContainer = root.Q<VisualElement>("buttons-container");
        pagesButtonContainer = root.Q<VisualElement>("pages-button");
        nextPageButton = root.Q<Button>("next-page-button");
        previousPageButton = root.Q<Button>("previous-page-button");
        VisualElement guidesListRoot = root.Q<VisualElement>("guide-list-scroll");
        guidesListContainer = guidesListRoot.Q<VisualElement>("unity-content-container");


        if (nextPageButton != null) {
            nextPageButton.clickable.clicked += () => NavigatePage(1);
        }

        if (previousPageButton != null) {
            previousPageButton.clickable.clicked += () => NavigatePage(-1);
        }

        await SetUp();
    }

    public async Awaitable SetUp() {

        guidesListContainer.Clear();
        guideButtons.Clear();

        AsyncOperationHandle<IList<GuideSO>> loadHandle = Addressables.LoadAssetsAsync<GuideSO>(guideLabel, null);

        await loadHandle.Task;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded) {
            IList<GuideSO> allGuides = loadHandle.Result;

            foreach (GuideSO guide in allGuides) {
                Button guideButton = new Button();
                guideButton.AddToClassList("simple-button");
                guideButton.text = guide.guideTitle;

                guideButton.clickable.clicked += () => OnClickGuideButton(guide);

                guideButtons.Add(guideButton);
                guidesListContainer.Add(guideButton);
            }
        }
    }

    public void OnClickGuideButton(GuideSO guide) {
        currentGuide = guide;
        currentPageIndex = 0;

        titleLabel.text = guide.guideTitle;

        if (guide.Pages.Count > 0) {
            UpdatePage(guide.Pages[currentPageIndex]);
        }
        else {
            UpdatePage(new Page { descriptionText = "Este guia está vazio.", pageSprite = null });
        }

        bool hasMultiplePages = guide.Pages.Count > 1;
        buttonsContainer.style.display = hasMultiplePages ? DisplayStyle.Flex : DisplayStyle.None;

        ClearPageButtons();

        if (hasMultiplePages) {
            CreatePageButtons();
            UpdateNavigationState();
        }
    }

    private void ClearPageButtons() {
        foreach (var button in pageButtons) {
            button.RemoveFromHierarchy();
        }
        pageButtons.Clear();
    }

    private void CreatePageButtons() {
        for (int i = 0; i < currentGuide.Pages.Count; i++) {
            Button pageButton = new Button();
            //pageButton.text = (i + 1).ToString(); 

            pageButton.AddToClassList("page-non-selected-button");

            int index = i;
            pageButton.clickable.clicked += () => NavigateToPage(index);

            pagesButtonContainer.Add(pageButton);
            pageButtons.Add(pageButton);
        }
    }

    private void NavigatePage(int delta) {
        NavigateToPage(currentPageIndex + delta);
    }

    private void NavigateToPage(int index) {
        if (currentGuide == null || index < 0 || index >= currentGuide.Pages.Count) {
            return;
        }

        currentPageIndex = index;
        UpdatePage(currentGuide.Pages[currentPageIndex]);

        UpdateNavigationState();
    }

    private void UpdateNavigationState() {
        if (currentGuide == null) return;

        int totalPages = currentGuide.Pages.Count;

        nextPageButton.SetEnabled(currentPageIndex < totalPages - 1);
        previousPageButton.SetEnabled(currentPageIndex > 0);

        for (int i = 0; i < pageButtons.Count; i++) {
            if (i == currentPageIndex) {
                pageButtons[i].RemoveFromClassList("page-non-selected-button");
                pageButtons[i].AddToClassList("page-selected-button");
            }
            else {
                pageButtons[i].RemoveFromClassList("page-selected-button");
                pageButtons[i].AddToClassList("page-non-selected-button");
            }
        }
    }

    public void UpdatePage(Page page) {
        descriptionLabel.text = page.descriptionText;

        if (pageImageContainer != null) {
            if (page.pageSprite != null) {
                pageImageContainer.style.backgroundImage = new StyleBackground(page.pageSprite);
            }
            else {
                pageImageContainer.style.backgroundImage = new StyleBackground(StyleKeyword.None);
            }
        }
    }
}