using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DefendArtsystackUiBuilder
{
    private const string _vendorRoot = "Assets/Artsystack - Fantasy RPG GUI";
    private const string _generatedRoot = "Assets/_LocalGenerated/DefendUI";
    private const string _prefabsRoot = _generatedRoot + "/Prefabs";
    private const string _configsRoot = _generatedRoot + "/Configs";
    private const string _buttonReportPath = "Docs/ButtonAutoWiringReport.md";

    private static readonly string[] _mainMenuBackgroundKeywords = { "background", "bg" };
    private static readonly string[] _mainMenuPanelKeywords = { "panel", "window", "frame" };
    private static readonly string[] _mainMenuButtonKeywords = { "button", "btn" };
    private static readonly string[] _mainMenuPlayKeywords = { "play" };
    private static readonly string[] _mainMenuTitleKeywords = { "title" };

    private static readonly string[] _gameplayPanelKeywords = { "panel", "frame", "hud" };
    private static readonly string[] _gameplayIconKeywords = { "icon" };
    private static readonly string[] _gameplayGoldKeywords = { "gold", "coin" };
    private static readonly string[] _gameplayDiamondKeywords = { "diamond", "gem", "crystal" };
    private static readonly string[] _gameplayWaveKeywords = { "wave" };
    private static readonly string[] _gameplayHealthKeywords = { "health", "hp" };
    private static readonly string[] _gameplayBarKeywords = { "bar", "progress" };

    private static readonly string[] _placementSlotKeywords = { "slot", "card", "button" };
    private static readonly string[] _placementMineKeywords = { "mine" };
    private static readonly string[] _placementTurretKeywords = { "turret", "tower" };
    private static readonly string[] _placementPuddleKeywords = { "puddle" };
    private static readonly string[] _placementBuildKeywords = { "build", "upgrade" };

    private static readonly string[] _resultPopupKeywords = { "popup", "modal", "window", "panel" };
    private static readonly string[] _resultWinKeywords = { "win" };
    private static readonly string[] _resultLoseKeywords = { "lose" };
    private static readonly string[] _resultButtonKeywords = { "button", "restart", "menu" };
    private static readonly string[] _fontKeywords = { "sdf", "regular", "font", "medieval", "kurale" };

    private static readonly HashSet<string> _optionalGeneratedSkinFields = new HashSet<string>
    {
        "_optionalSkinRoot",
        "_backgroundImage",
        "_frameImage",
        "_iconImage",
        "_buttonImage",
        "_label"
    };

    [MenuItem("Tools/Defend/Build Artsystack UI")]
    public static void BuildArtsystackUi()
    {
        if (AssetDatabase.IsValidFolder(_vendorRoot) == false)
        {
            Debug.LogError($"Artsystack UI pack was not found at '{_vendorRoot}'. Import it locally, keep it ignored, then run this tool again.");
            return;
        }

        EnsureGeneratedFolders();

        AssetCatalog catalog = ScanAssetCatalog();
        CandidateSelection selection = SelectCandidates(catalog);

        LogCatalogSummary(catalog);
        LogSelectedCandidates(selection);

        CreateMainMenuPanel(selection);
        CreateGameplayHudPanel(selection);
        CreatePlacementPanelSkin(selection);
        CreateResultPopupSkin(selection);
        CreateTowerButtonSkin(selection);
        CreateCurrencyRowSkin(selection);
        CreateLocalIconConfig(selection);
        CreateLocalPresentationFeedbackConfig();
        WriteButtonAutoWiringReport();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Defend Artsystack UI build finished. Generated assets are local-only under Assets/_LocalGenerated/DefendUI. Scenes and vendor assets were not modified.");
        Debug.Log("Generated buttons are visual-only. See Docs/ButtonAutoWiringReport.md for exact manual wiring steps.");
    }

    [MenuItem("Tools/Defend/Validate Active UI Setup")]
    public static void ValidateActiveUiSetup()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.IsValid() == false)
        {
            Debug.LogError("No valid active scene is open.");
            return;
        }

        Debug.Log($"Validating active UI setup in scene '{activeScene.name}'. This editor validation is read-only.");

        List<DefendGameplayScreenView> gameplayScreens = FindSceneObjects<DefendGameplayScreenView>(activeScene);
        List<DefendHudView> hudViews = FindSceneObjects<DefendHudView>(activeScene);
        List<PlacementPanelView> placementPanels = FindSceneObjects<PlacementPanelView>(activeScene);
        List<CurrencyListView> currencyLists = FindSceneObjects<CurrencyListView>(activeScene);
        List<PopupLayer> popupLayers = FindSceneObjects<PopupLayer>(activeScene);
        List<MainMenuScreenView> mainMenuScreens = FindSceneObjects<MainMenuScreenView>(activeScene);
        List<MainMenuView> mainMenus = FindSceneObjects<MainMenuView>(activeScene);
        List<MessagePopupView> messagePopups = FindSceneObjects<MessagePopupView>(activeScene);

        LogComponentCount(nameof(DefendGameplayScreenView), gameplayScreens.Count);
        LogComponentCount(nameof(DefendHudView), hudViews.Count);
        LogComponentCount(nameof(PlacementPanelView), placementPanels.Count);
        LogComponentCount(nameof(CurrencyListView), currencyLists.Count);
        LogComponentCount(nameof(PopupLayer), popupLayers.Count);
        LogComponentCount(nameof(MainMenuScreenView), mainMenuScreens.Count);

        ValidateComponents(gameplayScreens);
        ValidateComponents(hudViews);
        ValidateComponents(placementPanels);
        ValidateComponents(currencyLists);
        ValidateComponents(popupLayers);
        ValidateComponents(mainMenuScreens);
        ValidateComponents(mainMenus);
        ValidateComponents(messagePopups);

        ValidateViewButtonFields(mainMenus, "_playButton", "Main Menu Play");
        ValidateViewButtonFields(placementPanels, "_mineButton", "Mine");
        ValidateViewButtonFields(placementPanels, "_turretButton", "Turret");
        ValidateViewButtonFields(placementPanels, "_puddleButton", "Puddle");
        ValidateViewButtonFields(messagePopups, "_okButton", "Result OK/Close");

        ValidateNamedSceneButtons(activeScene, "Start Wave / Continue", new[] { "start", "wave", "continue" });
        ValidateNamedSceneButtons(activeScene, "Restart", new[] { "restart" });
        ValidateNamedSceneButtons(activeScene, "Return to Menu", new[] { "return", "menu" });

        Debug.Log("Active UI setup validation finished.");
    }

    private static void EnsureGeneratedFolders()
    {
        EnsureFolder("Assets/_LocalGenerated");
        EnsureFolder(_generatedRoot);
        EnsureFolder(_prefabsRoot);
        EnsureFolder(_configsRoot);
    }

    private static void EnsureFolder(string assetFolder)
    {
        if (AssetDatabase.IsValidFolder(assetFolder))
        {
            return;
        }

        string parent = Path.GetDirectoryName(assetFolder);

        if (string.IsNullOrEmpty(parent))
        {
            Debug.LogError($"Cannot create Unity asset folder '{assetFolder}'.");
            return;
        }

        parent = parent.Replace("\\", "/");
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, Path.GetFileName(assetFolder));
    }

    private static AssetCatalog ScanAssetCatalog()
    {
        AssetCatalog catalog = new AssetCatalog();

        catalog.Sprites.AddRange(ScanAssets<Sprite>("t:Sprite"));
        catalog.Prefabs.AddRange(ScanAssets<GameObject>("t:Prefab"));
        catalog.TmpFonts.AddRange(ScanAssets<TMP_FontAsset>("t:TMP_FontAsset"));
        catalog.Fonts.AddRange(ScanAssets<Font>("t:Font"));

        return catalog;
    }

    private static List<AssetCandidate> ScanAssets<T>(string filter)
        where T : UnityEngine.Object
    {
        List<AssetCandidate> candidates = new List<AssetCandidate>();
        string[] guids = AssetDatabase.FindAssets(filter, new[] { _vendorRoot });

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = LoadAssetAtPath<T>(path);

            if (asset == null)
            {
                continue;
            }

            AssetCandidate candidate = new AssetCandidate
            {
                Asset = asset,
                Name = Path.GetFileNameWithoutExtension(path),
                Path = path
            };

            candidates.Add(candidate);
        }

        return candidates;
    }

    private static T LoadAssetAtPath<T>(string path)
        where T : UnityEngine.Object
    {
        T asset = AssetDatabase.LoadAssetAtPath<T>(path);

        if (asset != null)
        {
            return asset;
        }

        UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);

        for (int i = 0; i < assets.Length; i++)
        {
            T typedAsset = assets[i] as T;

            if (typedAsset != null)
            {
                return typedAsset;
            }
        }

        return null;
    }

    private static CandidateSelection SelectCandidates(AssetCatalog catalog)
    {
        CandidateSelection selection = new CandidateSelection();

        selection.Add("MainMenuBackground", SelectBest(catalog.Sprites, "Main menu background", _mainMenuBackgroundKeywords));
        selection.Add("MainMenuPanel", SelectBest(catalog.Sprites, "Main menu panel/frame", _mainMenuPanelKeywords));
        selection.Add("MainMenuButton", SelectBest(catalog.Sprites, "Main menu button", _mainMenuButtonKeywords));
        selection.Add("MainMenuPlayIcon", SelectBest(catalog.Sprites, "Main menu play icon", _mainMenuPlayKeywords));
        selection.Add("MainMenuTitle", SelectBest(catalog.Sprites, "Main menu title accent", _mainMenuTitleKeywords));

        selection.Add("GameplayPanel", SelectBest(catalog.Sprites, "Gameplay HUD panel/frame", _gameplayPanelKeywords));
        selection.Add("GameplayIconFrame", SelectBest(catalog.Sprites, "Gameplay HUD icon frame", _gameplayIconKeywords));
        selection.Add("GoldIcon", SelectBest(catalog.Sprites, "Gold currency icon", _gameplayGoldKeywords));
        selection.Add("DiamondIcon", SelectBest(catalog.Sprites, "Diamond currency icon", _gameplayDiamondKeywords));
        selection.Add("WaveIcon", SelectBest(catalog.Sprites, "Wave indicator icon", _gameplayWaveKeywords));
        selection.Add("HealthIcon", SelectBest(catalog.Sprites, "Base health icon", _gameplayHealthKeywords));
        selection.Add("ProgressBar", SelectBest(catalog.Sprites, "Base health progress bar", _gameplayBarKeywords));

        selection.Add("PlacementSlot", SelectBest(catalog.Sprites, "Placement slot/card/button", _placementSlotKeywords));
        selection.Add("MineIcon", SelectBest(catalog.Sprites, "Mine placement icon", _placementMineKeywords));
        selection.Add("TurretIcon", SelectBest(catalog.Sprites, "Turret/tower placement icon", _placementTurretKeywords));
        selection.Add("PuddleIcon", SelectBest(catalog.Sprites, "Puddle placement icon", _placementPuddleKeywords));
        selection.Add("BuildIcon", SelectBest(catalog.Sprites, "Build/upgrade icon", _placementBuildKeywords));

        selection.Add("ResultPopupPanel", SelectBest(catalog.Sprites, "Result popup panel", _resultPopupKeywords));
        selection.Add("ResultWinIcon", SelectBest(catalog.Sprites, "Victory result icon", _resultWinKeywords));
        selection.Add("ResultLoseIcon", SelectBest(catalog.Sprites, "Defeat result icon", _resultLoseKeywords));
        selection.Add("ResultButton", SelectBest(catalog.Sprites, "Result popup button", _resultButtonKeywords));

        selection.Add("UiFont", SelectBest(catalog.TmpFonts, "TextMesh Pro UI font", _fontKeywords));
        selection.Add("ReferenceMainMenuPrefab", SelectBest(catalog.Prefabs, "Reference-only main menu prefab", new[] { "title", "tap", "start", "gameplay" }));
        selection.Add("ReferencePopupPrefab", SelectBest(catalog.Prefabs, "Reference-only popup prefab", new[] { "popup", "pause", "update" }));
        selection.Add("ReferenceFontFile", SelectBest(catalog.Fonts, "Reference-only font file", _fontKeywords));

        return selection;
    }

    private static MatchedAsset SelectBest(List<AssetCandidate> candidates, string usage, string[] keywords)
    {
        AssetCandidate bestCandidate = null;
        string bestKeyword = string.Empty;
        int bestScore = 0;

        for (int i = 0; i < candidates.Count; i++)
        {
            AssetCandidate candidate = candidates[i];

            for (int keywordIndex = 0; keywordIndex < keywords.Length; keywordIndex++)
            {
                string keyword = keywords[keywordIndex];
                int score = ScoreCandidate(candidate, keyword, keywordIndex);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestCandidate = candidate;
                    bestKeyword = keyword;
                }
            }
        }

        if (bestCandidate == null)
        {
            Debug.LogWarning($"No Artsystack candidate found for {usage}. The generated UI will use simple fallback colors for that slot.");
            return null;
        }

        return new MatchedAsset
        {
            Asset = bestCandidate.Asset,
            Keyword = bestKeyword,
            Path = bestCandidate.Path,
            Score = bestScore,
            Usage = usage
        };
    }

    private static int ScoreCandidate(AssetCandidate candidate, string keyword, int keywordIndex)
    {
        string normalizedPath = Normalize(candidate.Path);
        string normalizedName = Normalize(candidate.Name);
        string normalizedKeyword = Normalize(keyword);

        if (MatchesKeyword(normalizedPath, normalizedKeyword) == false)
        {
            return 0;
        }

        int score = 100 - (keywordIndex * 4);

        if (MatchesKeyword(normalizedName, normalizedKeyword))
        {
            score += 50;
        }

        if (normalizedName == normalizedKeyword)
        {
            score += 100;
        }

        if (normalizedPath.Contains("/" + normalizedKeyword + "/"))
        {
            score += 35;
        }

        return score;
    }

    private static string Normalize(string value)
    {
        return (value ?? string.Empty).Replace("\\", "/").ToLowerInvariant();
    }

    private static bool MatchesKeyword(string value, string keyword)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(keyword))
        {
            return false;
        }

        if (keyword.Length > 2)
        {
            return value.Contains(keyword);
        }

        string[] tokens = value.Split(new[] { '/', '\\', '_', '-', ' ', '.', '(', ')', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i] == keyword)
            {
                return true;
            }
        }

        return false;
    }

    private static void LogCatalogSummary(AssetCatalog catalog)
    {
        Debug.Log(
            $"Artsystack scan complete: {catalog.Sprites.Count} sprites, {catalog.Prefabs.Count} prefabs, " +
            $"{catalog.TmpFonts.Count} TMP font assets, {catalog.Fonts.Count} font files.");
    }

    private static void LogSelectedCandidates(CandidateSelection selection)
    {
        List<MatchedAsset> matches = selection.GetMatches();

        for (int i = 0; i < matches.Count; i++)
        {
            MatchedAsset match = matches[i];
            Debug.Log($"Artsystack selected: {match.Path} | keyword '{match.Keyword}' | usage: {match.Usage}.");
        }
    }

    private static void CreateMainMenuPanel(CandidateSelection selection)
    {
        GameObject root = CreateCanvasRoot("Generated_MainMenuPanel");
        RectTransform rootRect = root.GetComponent<RectTransform>();

        RectTransform background = CreateUiObject("Background", rootRect);
        Stretch(background, 0f, 0f, 0f, 0f);
        AddImage(background.gameObject, selection.Sprite("MainMenuBackground"), new Color32(35, 31, 43, 255), false);

        RectTransform panel = CreateUiObject("CenterPanel", rootRect);
        AnchorCenter(panel, new Vector2(720f, 560f), Vector2.zero);
        AddImage(panel.gameObject, selection.Sprite("MainMenuPanel"), new Color32(63, 45, 58, 235), true);

        VerticalLayoutGroup layout = panel.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(64, 64, 54, 54);
        layout.spacing = 26f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        RectTransform title = CreateUiObject("Title", panel);
        AddLayoutElement(title.gameObject, 0f, 96f);
        AddText(title, "Tower Defence", 64f, TextAlignmentOptions.Center, new Color32(255, 239, 181, 255), selection);

        RectTransform subtitle = CreateUiObject("Subtitle", panel);
        AddLayoutElement(subtitle.gameObject, 0f, 46f);
        AddText(subtitle, "Fantasy Outpost", 30f, TextAlignmentOptions.Center, new Color32(222, 214, 191, 255), selection);

        Button playButton = CreateButton(panel, "PlayButton", "Play", selection.Sprite("MainMenuButton"), selection);
        AddLayoutElement(playButton.gameObject, 420f, 90f);

        Button upgradesButton = CreateButton(panel, "UpgradesButton", "Upgrades", selection.Sprite("MainMenuButton"), selection);
        AddLayoutElement(upgradesButton.gameObject, 360f, 72f);

        Button resetButton = CreateButton(panel, "ResetButton", "Reset", selection.Sprite("MainMenuButton"), selection);
        AddLayoutElement(resetButton.gameObject, 360f, 72f);

        SaveGeneratedPrefab(root, _prefabsRoot + "/Generated_MainMenuPanel.prefab");
    }

    private static void CreateGameplayHudPanel(CandidateSelection selection)
    {
        GameObject root = CreateCanvasRoot("Generated_GameplayHudPanel");
        RectTransform rootRect = root.GetComponent<RectTransform>();

        RectTransform topBar = CreateUiObject("TopBar", rootRect);
        AnchorTopStretch(topBar, 24f, 24f, 24f, 96f);
        AddImage(topBar.gameObject, selection.Sprite("GameplayPanel"), new Color32(41, 38, 48, 230), true);

        HorizontalLayoutGroup topLayout = topBar.gameObject.AddComponent<HorizontalLayoutGroup>();
        topLayout.padding = new RectOffset(28, 28, 16, 16);
        topLayout.spacing = 20f;
        topLayout.childAlignment = TextAnchor.MiddleCenter;
        topLayout.childControlHeight = true;
        topLayout.childControlWidth = false;
        topLayout.childForceExpandHeight = true;
        topLayout.childForceExpandWidth = false;

        CreateStatPill(topBar, "GoldCurrency", "Gold", "0", selection.Sprite("GoldIcon"), selection, 250f);
        CreateStatPill(topBar, "DiamondCurrency", "Diamonds", "0", selection.Sprite("DiamondIcon"), selection, 270f);
        CreateStatPill(topBar, "WaveStatus", "Wave", "1/3", selection.Sprite("WaveIcon"), selection, 250f);
        CreateStatPill(topBar, "PhaseStatus", "Status", "Build", selection.Sprite("GameplayIconFrame"), selection, 300f);
        CreateHealthPill(topBar, selection);

        RectTransform startWavePanel = CreateUiObject("WaveActionPanel", rootRect);
        AnchorBottomRight(startWavePanel, new Vector2(340f, 96f), new Vector2(-32f, 32f));
        AddImage(startWavePanel.gameObject, selection.Sprite("GameplayPanel"), new Color32(41, 38, 48, 220), true);

        Button startWaveButton = CreateButton(startWavePanel, "StartWaveButton", "Start Wave", selection.Sprite("MainMenuButton"), selection);
        Stretch(startWaveButton.GetComponent<RectTransform>(), 16f, 14f, 16f, 14f);

        SaveGeneratedPrefab(root, _prefabsRoot + "/Generated_GameplayHudPanel.prefab");
    }

    private static void CreatePlacementPanelSkin(CandidateSelection selection)
    {
        GameObject root = CreatePanelRoot("Generated_PlacementPanelSkin", new Vector2(940f, 236f));
        RectTransform rootRect = root.GetComponent<RectTransform>();

        AddImage(root, selection.Sprite("GameplayPanel"), new Color32(46, 40, 50, 235), true);

        VerticalLayoutGroup layout = root.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(26, 26, 18, 22);
        layout.spacing = 14f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        RectTransform title = CreateUiObject("Title", rootRect);
        AddLayoutElement(title.gameObject, 0f, 32f);
        AddText(title, "Build", 28f, TextAlignmentOptions.Center, new Color32(255, 239, 181, 255), selection);

        RectTransform buttons = CreateUiObject("Buttons", rootRect);
        AddLayoutElement(buttons.gameObject, 0f, 142f);
        HorizontalLayoutGroup buttonLayout = buttons.gameObject.AddComponent<HorizontalLayoutGroup>();
        buttonLayout.spacing = 18f;
        buttonLayout.childAlignment = TextAnchor.MiddleCenter;
        buttonLayout.childControlHeight = true;
        buttonLayout.childControlWidth = true;
        buttonLayout.childForceExpandHeight = true;
        buttonLayout.childForceExpandWidth = true;

        CreateTowerButton(buttons, "MineButton", "Mine", "25 Gold", selection.Sprite("MineIcon"), selection);
        CreateTowerButton(buttons, "TurretButton", "Turret", "50 Gold", selection.Sprite("TurretIcon"), selection);
        CreateTowerButton(buttons, "PuddleButton", "Puddle", "35 Gold", selection.Sprite("PuddleIcon"), selection);

        SaveGeneratedPrefab(root, _prefabsRoot + "/Generated_PlacementPanelSkin.prefab");
    }

    private static void CreateResultPopupSkin(CandidateSelection selection)
    {
        GameObject root = CreateCanvasRoot("Generated_ResultPopupSkin");
        RectTransform rootRect = root.GetComponent<RectTransform>();

        RectTransform anticlicker = CreateUiObject("Anticlicker", rootRect);
        Stretch(anticlicker, 0f, 0f, 0f, 0f);
        AddImage(anticlicker.gameObject, null, new Color32(9, 8, 12, 175), true);

        RectTransform body = CreateUiObject("Body", rootRect);
        AnchorCenter(body, new Vector2(760f, 480f), Vector2.zero);
        AddImage(body.gameObject, selection.Sprite("ResultPopupPanel"), new Color32(59, 47, 57, 245), true);

        VerticalLayoutGroup layout = body.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(58, 58, 48, 42);
        layout.spacing = 22f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        RectTransform title = CreateUiObject("TitleText", body);
        AddLayoutElement(title.gameObject, 0f, 72f);
        AddText(title, "Victory", 52f, TextAlignmentOptions.Center, new Color32(255, 239, 181, 255), selection);

        RectTransform message = CreateUiObject("MessageText", body);
        AddLayoutElement(message.gameObject, 0f, 120f);
        AddText(message, "Base defended.", 30f, TextAlignmentOptions.Center, new Color32(224, 220, 203, 255), selection);

        RectTransform buttons = CreateUiObject("Buttons", body);
        AddLayoutElement(buttons.gameObject, 0f, 88f);
        HorizontalLayoutGroup buttonLayout = buttons.gameObject.AddComponent<HorizontalLayoutGroup>();
        buttonLayout.spacing = 18f;
        buttonLayout.childAlignment = TextAnchor.MiddleCenter;
        buttonLayout.childControlHeight = true;
        buttonLayout.childControlWidth = true;
        buttonLayout.childForceExpandHeight = true;
        buttonLayout.childForceExpandWidth = true;

        CreateButton(buttons, "OkButton", "OK", selection.Sprite("ResultButton"), selection);
        CreateButton(buttons, "RestartButton", "Restart", selection.Sprite("ResultButton"), selection);
        CreateButton(buttons, "ReturnToMenuButton", "Menu", selection.Sprite("ResultButton"), selection);

        Button closeButton = CreateButton(body, "CloseButton", "X", selection.Sprite("ResultButton"), selection);
        LayoutElement closeLayout = closeButton.gameObject.AddComponent<LayoutElement>();
        closeLayout.ignoreLayout = true;

        RectTransform closeRect = closeButton.GetComponent<RectTransform>();
        closeRect.SetParent(body, false);
        AnchorTopRight(closeRect, new Vector2(56f, 56f), new Vector2(-28f, -24f));

        SaveGeneratedPrefab(root, _prefabsRoot + "/Generated_ResultPopupSkin.prefab");
    }

    private static void CreateTowerButtonSkin(CandidateSelection selection)
    {
        GameObject root = CreatePanelRoot("Generated_TowerButtonSkin", new Vector2(260f, 156f));
        CreateTowerButton(root.transform, "TowerButton", "Tower", "50 Gold", selection.Sprite("TurretIcon"), selection);
        Stretch(root.transform.GetChild(0).GetComponent<RectTransform>(), 0f, 0f, 0f, 0f);
        SaveGeneratedPrefab(root, _prefabsRoot + "/Generated_TowerButtonSkin.prefab");
    }

    private static void CreateCurrencyRowSkin(CandidateSelection selection)
    {
        GameObject root = CreatePanelRoot("Generated_CurrencyRowSkin", new Vector2(300f, 72f));
        RectTransform rootRect = root.GetComponent<RectTransform>();
        AddImage(root, selection.Sprite("GameplayPanel"), new Color32(45, 39, 47, 225), true);

        HorizontalLayoutGroup layout = root.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(16, 18, 10, 10);
        layout.spacing = 12f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = true;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = true;
        layout.childForceExpandWidth = false;

        RectTransform icon = CreateUiObject("IconImage", rootRect);
        AddLayoutElement(icon.gameObject, 48f, 48f);
        Image iconImage = AddImage(icon.gameObject, selection.Sprite("GoldIcon"), Color.white, false);
        iconImage.preserveAspect = true;

        RectTransform name = CreateUiObject("NameText", rootRect);
        AddLayoutElement(name.gameObject, 100f, 42f);
        AddText(name, "Gold", 24f, TextAlignmentOptions.Left, new Color32(235, 225, 198, 255), selection);

        RectTransform amount = CreateUiObject("AmountText", rootRect);
        AddLayoutElement(amount.gameObject, 90f, 42f);
        AddText(amount, "0", 26f, TextAlignmentOptions.Right, new Color32(255, 239, 181, 255), selection);

        SaveGeneratedPrefab(root, _prefabsRoot + "/Generated_CurrencyRowSkin.prefab");
    }

    private static void CreateLocalIconConfig(CandidateSelection selection)
    {
        string path = _configsRoot + "/DefendUiIconConfig.asset";
        DefendUiIconConfig config = AssetDatabase.LoadAssetAtPath<DefendUiIconConfig>(path);

        if (config != null && ConfirmReplace(path, "Update generated icon config") == false)
        {
            Debug.LogWarning($"Skipped existing generated config: {path}");
            return;
        }

        if (config == null)
        {
            config = ScriptableObject.CreateInstance<DefendUiIconConfig>();
            AssetDatabase.CreateAsset(config, path);
        }

        SerializedObject serializedObject = new SerializedObject(config);
        SetCurrencyEntries(serializedObject, selection);
        SetPlaceableEntries(serializedObject, selection);
        SetPhaseEntries(serializedObject, selection);
        SetEmptyArray(serializedObject, "_enemyEntries");
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(config);

        Debug.Log($"Generated local icon config: {path}. It is ignored and must not replace the tracked Resources config automatically.");
    }

    private static void CreateLocalPresentationFeedbackConfig()
    {
        string path = _configsRoot + "/DefendPresentationFeedbackConfig.asset";
        DefendPresentationFeedbackConfig config = AssetDatabase.LoadAssetAtPath<DefendPresentationFeedbackConfig>(path);

        if (config != null && ConfirmReplace(path, "Update generated feedback config") == false)
        {
            Debug.LogWarning($"Skipped existing generated config: {path}");
            return;
        }

        if (config == null)
        {
            config = ScriptableObject.CreateInstance<DefendPresentationFeedbackConfig>();
            AssetDatabase.CreateAsset(config, path);
        }

        SerializedObject serializedObject = new SerializedObject(config);
        SetEmptyArray(serializedObject, "_sfxCues");
        SetEmptyArray(serializedObject, "_vfxCues");
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(config);

        Debug.Log($"Generated local feedback config shell: {path}. No safe Artsystack SFX/VFX candidates are assigned automatically.");
    }

    private static void SetCurrencyEntries(SerializedObject serializedObject, CandidateSelection selection)
    {
        SerializedProperty entries = serializedObject.FindProperty("_currencyEntries");
        entries.arraySize = 2;
        SetEntry(entries.GetArrayElementAtIndex(0), "_type", (int)CurrencyType.Gold, "_displayName", "Gold", "_icon", selection.Sprite("GoldIcon"));
        SetEntry(entries.GetArrayElementAtIndex(1), "_type", (int)CurrencyType.Diamond, "_displayName", "Diamonds", "_icon", selection.Sprite("DiamondIcon"));
    }

    private static void SetPlaceableEntries(SerializedObject serializedObject, CandidateSelection selection)
    {
        SerializedProperty entries = serializedObject.FindProperty("_placeableEntries");
        entries.arraySize = 3;
        SetEntry(entries.GetArrayElementAtIndex(0), "_type", (int)PlaceableType.Mine, "_displayName", "Mine", "_icon", selection.Sprite("MineIcon"));
        SetEntry(entries.GetArrayElementAtIndex(1), "_type", (int)PlaceableType.Turret, "_displayName", "Turret", "_icon", selection.Sprite("TurretIcon"));
        SetEntry(entries.GetArrayElementAtIndex(2), "_type", (int)PlaceableType.Puddle, "_displayName", "Puddle", "_icon", selection.Sprite("PuddleIcon"));
    }

    private static void SetPhaseEntries(SerializedObject serializedObject, CandidateSelection selection)
    {
        SerializedProperty entries = serializedObject.FindProperty("_phaseEntries");
        entries.arraySize = 3;
        SetEntry(entries.GetArrayElementAtIndex(0), "_phase", (int)DefendPhase.Wave, "_displayName", "Wave", "_icon", selection.Sprite("WaveIcon"));
        SetEntry(entries.GetArrayElementAtIndex(1), "_phase", (int)DefendPhase.Rest, "_displayName", "Build", "_icon", selection.Sprite("BuildIcon"));
        SetEntry(entries.GetArrayElementAtIndex(2), "_phase", (int)DefendPhase.Ended, "_displayName", "Ended", "_icon", selection.Sprite("ResultWinIcon"));
    }

    private static void SetEntry(
        SerializedProperty entry,
        string enumField,
        int enumValue,
        string displayNameField,
        string displayName,
        string iconField,
        Sprite icon)
    {
        SerializedProperty enumProperty = entry.FindPropertyRelative(enumField);
        SerializedProperty displayNameProperty = entry.FindPropertyRelative(displayNameField);
        SerializedProperty iconProperty = entry.FindPropertyRelative(iconField);

        SetEnumValue(enumProperty, enumValue);
        displayNameProperty.stringValue = displayName;
        iconProperty.objectReferenceValue = icon;
    }

    private static void SetEnumValue(SerializedProperty property, int value)
    {
        if (property.propertyType == SerializedPropertyType.Enum)
        {
            property.enumValueIndex = Mathf.Clamp(value, 0, property.enumDisplayNames.Length - 1);
            return;
        }

        property.intValue = value;
    }

    private static void SetEmptyArray(SerializedObject serializedObject, string propertyName)
    {
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property != null && property.isArray)
        {
            property.arraySize = 0;
        }
    }

    private static GameObject CreateCanvasRoot(string name)
    {
        GameObject root = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        RectTransform rect = root.GetComponent<RectTransform>();
        Stretch(rect, 0f, 0f, 0f, 0f);

        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        return root;
    }

    private static GameObject CreatePanelRoot(string name, Vector2 size)
    {
        GameObject root = new GameObject(name, typeof(RectTransform));
        RectTransform rect = root.GetComponent<RectTransform>();
        AnchorCenter(rect, size, Vector2.zero);
        return root;
    }

    private static RectTransform CreateUiObject(string name, Transform parent)
    {
        GameObject gameObject = new GameObject(name, typeof(RectTransform));
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.SetParent(parent, false);
        rectTransform.localScale = Vector3.one;
        return rectTransform;
    }

    private static Image AddImage(GameObject gameObject, Sprite sprite, Color color, bool raycastTarget)
    {
        Image image = gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.raycastTarget = raycastTarget;

        if (sprite != null && sprite.border.sqrMagnitude > 0f)
        {
            image.type = Image.Type.Sliced;
        }

        return image;
    }

    private static TextMeshProUGUI AddText(
        RectTransform parent,
        string value,
        float fontSize,
        TextAlignmentOptions alignment,
        Color color,
        CandidateSelection selection)
    {
        RectTransform textRect = parent;
        TextMeshProUGUI text = textRect.gameObject.AddComponent<TextMeshProUGUI>();
        TMP_FontAsset font = selection.TmpFont("UiFont");

        if (font != null)
        {
            text.font = font;
        }

        text.text = value;
        text.fontSize = fontSize;
        text.enableAutoSizing = true;
        text.fontSizeMin = Mathf.Max(12f, fontSize * 0.55f);
        text.fontSizeMax = fontSize;
        text.alignment = alignment;
        text.color = color;
        text.raycastTarget = false;
        text.overflowMode = TextOverflowModes.Ellipsis;

        return text;
    }

    private static Button CreateButton(Transform parent, string name, string label, Sprite sprite, CandidateSelection selection)
    {
        RectTransform buttonRect = CreateUiObject(name, parent);
        Image image = AddImage(buttonRect.gameObject, sprite, new Color32(99, 71, 72, 255), true);

        Button button = buttonRect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.ColorTint;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color32(255, 239, 181, 255);
        colors.pressedColor = new Color32(205, 176, 120, 255);
        colors.selectedColor = new Color32(255, 239, 181, 255);
        colors.disabledColor = new Color32(117, 103, 106, 160);
        colors.colorMultiplier = 1f;
        colors.fadeDuration = 0.1f;
        button.colors = colors;

        if (string.IsNullOrEmpty(label) == false)
        {
            RectTransform labelRect = CreateUiObject("Label", buttonRect);
            Stretch(labelRect, 18f, 8f, 18f, 8f);
            AddText(labelRect, label, 30f, TextAlignmentOptions.Center, new Color32(255, 242, 211, 255), selection);
        }

        return button;
    }

    private static void CreateStatPill(
        Transform parent,
        string name,
        string label,
        string value,
        Sprite iconSprite,
        CandidateSelection selection,
        float width)
    {
        RectTransform pill = CreateUiObject(name, parent);
        AddLayoutElement(pill.gameObject, width, 64f);
        AddImage(pill.gameObject, selection.Sprite("GameplayPanel"), new Color32(63, 50, 58, 230), true);

        HorizontalLayoutGroup layout = pill.gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(14, 16, 8, 8);
        layout.spacing = 10f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = true;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = true;
        layout.childForceExpandWidth = false;

        RectTransform icon = CreateUiObject("IconImage", pill);
        AddLayoutElement(icon.gameObject, 40f, 40f);
        Image iconImage = AddImage(icon.gameObject, iconSprite, Color.white, false);
        iconImage.preserveAspect = true;

        RectTransform text = CreateUiObject("Text", pill);
        AddLayoutElement(text.gameObject, width - 88f, 42f);
        AddText(text, $"{label}: {value}", 24f, TextAlignmentOptions.Left, new Color32(245, 234, 204, 255), selection);
    }

    private static void CreateHealthPill(Transform parent, CandidateSelection selection)
    {
        RectTransform pill = CreateUiObject("BaseHealth", parent);
        AddLayoutElement(pill.gameObject, 390f, 64f);
        AddImage(pill.gameObject, selection.Sprite("GameplayPanel"), new Color32(63, 50, 58, 230), true);

        HorizontalLayoutGroup layout = pill.gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(14, 16, 8, 8);
        layout.spacing = 12f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = true;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = true;
        layout.childForceExpandWidth = false;

        RectTransform label = CreateUiObject("BaseHpText", pill);
        AddLayoutElement(label.gameObject, 130f, 42f);
        AddText(label, "Base", 24f, TextAlignmentOptions.Left, new Color32(245, 234, 204, 255), selection);

        RectTransform sliderRoot = CreateUiObject("BaseHealthSlider", pill);
        AddLayoutElement(sliderRoot.gameObject, 200f, 34f);
        AddImage(sliderRoot.gameObject, selection.Sprite("ProgressBar"), new Color32(31, 27, 35, 255), false);

        Slider slider = sliderRoot.gameObject.AddComponent<Slider>();
        slider.interactable = false;
        slider.transition = Selectable.Transition.None;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0.8f;

        RectTransform fillArea = CreateUiObject("Fill Area", sliderRoot);
        Stretch(fillArea, 4f, 4f, 4f, 4f);

        RectTransform fill = CreateUiObject("Fill", fillArea);
        Stretch(fill, 0f, 0f, 0f, 0f);
        AddImage(fill.gameObject, null, new Color32(86, 203, 127, 255), false);
        slider.fillRect = fill;
    }

    private static void CreateTowerButton(
        Transform parent,
        string name,
        string label,
        string cost,
        Sprite iconSprite,
        CandidateSelection selection)
    {
        Button button = CreateButton(parent, name, string.Empty, selection.Sprite("PlacementSlot"), selection);
        AddLayoutElement(button.gameObject, 260f, 132f);
        RectTransform buttonRect = button.GetComponent<RectTransform>();

        RectTransform icon = CreateUiObject("IconImage", buttonRect);
        AnchorLeftCenter(icon, new Vector2(64f, 64f), new Vector2(24f, 0f));
        Image iconImage = AddImage(icon.gameObject, iconSprite, Color.white, false);
        iconImage.preserveAspect = true;

        RectTransform labelText = CreateUiObject("Label", buttonRect);
        AnchorTopStretch(labelText, 104f, 20f, 18f, 40f);
        AddText(labelText, label, 28f, TextAlignmentOptions.Left, new Color32(255, 242, 211, 255), selection);

        RectTransform costText = CreateUiObject("CostText", buttonRect);
        AnchorBottomStretch(costText, 104f, 20f, 18f, 38f);
        AddText(costText, cost, 22f, TextAlignmentOptions.Left, new Color32(219, 198, 139, 255), selection);
    }

    private static void AddLayoutElement(GameObject gameObject, float preferredWidth, float preferredHeight)
    {
        LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();

        if (preferredWidth > 0f)
        {
            layoutElement.preferredWidth = preferredWidth;
        }

        if (preferredHeight > 0f)
        {
            layoutElement.preferredHeight = preferredHeight;
        }
    }

    private static void Stretch(RectTransform rect, float left, float top, float right, float bottom)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = new Vector2(left, bottom);
        rect.offsetMax = new Vector2(-right, -top);
    }

    private static void AnchorCenter(RectTransform rect, Vector2 size, Vector2 position)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void AnchorTopStretch(RectTransform rect, float left, float top, float right, float height)
    {
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.sizeDelta = new Vector2(-(left + right), height);
        rect.anchoredPosition = new Vector2((left - right) * 0.5f, -top);
    }

    private static void AnchorBottomStretch(RectTransform rect, float left, float bottom, float right, float height)
    {
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = new Vector2(-(left + right), height);
        rect.anchoredPosition = new Vector2((left - right) * 0.5f, bottom);
    }

    private static void AnchorTopRight(RectTransform rect, Vector2 size, Vector2 position)
    {
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void AnchorBottomRight(RectTransform rect, Vector2 size, Vector2 position)
    {
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(1f, 0f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void AnchorLeftCenter(RectTransform rect, Vector2 size, Vector2 position)
    {
        rect.anchorMin = new Vector2(0f, 0.5f);
        rect.anchorMax = new Vector2(0f, 0.5f);
        rect.pivot = new Vector2(0f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void SaveGeneratedPrefab(GameObject root, string path)
    {
        try
        {
            if (File.Exists(path) && ConfirmReplace(path, "Replace generated prefab") == false)
            {
                Debug.LogWarning($"Skipped existing generated prefab: {path}");
                return;
            }

            PrefabUtility.SaveAsPrefabAsset(root, path, out bool success);

            if (success)
            {
                Debug.Log($"Generated local prefab: {path}");
            }
            else
            {
                Debug.LogError($"Failed to save generated prefab: {path}");
            }
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(root);
        }
    }

    private static bool ConfirmReplace(string path, string title)
    {
        return EditorUtility.DisplayDialog(
            title,
            $"{path} already exists. Replace/update this local generated asset?",
            "Replace",
            "Skip");
    }

    private static void WriteButtonAutoWiringReport()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Button Auto-Wiring Report");
        builder.AppendLine();
        builder.AppendLine("Generated UI prefabs are visual-only helpers. The builder does not attach gameplay decisions or persistent gameplay listeners to generated buttons.");
        builder.AppendLine();
        builder.AppendLine("| Button | Found in generated prefab | Safely wired automatically | Manual connection |");
        builder.AppendLine("| --- | --- | --- | --- |");
        builder.AppendLine("| Main Menu Play | `Generated_MainMenuPanel/CenterPanel/PlayButton` | No | Assign this `Button` to `MainMenuView._playButton`, or copy its `Image` sprite/target graphic onto the existing Play button. `MainMenuView.OnPlayClicked` already raises `PlayClicked`. |");
        builder.AppendLine("| Mine | `Generated_PlacementPanelSkin/Buttons/MineButton` | No | Assign to `PlacementPanelView._mineButton`. The existing `OnMineButtonClicked` handler raises `MineSelected`. |");
        builder.AppendLine("| Turret | `Generated_PlacementPanelSkin/Buttons/TurretButton` | No | Assign to `PlacementPanelView._turretButton`. The existing `OnTurretButtonClicked` handler raises `TurretSelected`. |");
        builder.AppendLine("| Puddle | `Generated_PlacementPanelSkin/Buttons/PuddleButton` | No | Assign to `PlacementPanelView._puddleButton`. The existing `OnPuddleButtonClicked` handler raises `PuddleSelected`. |");
        builder.AppendLine("| Start Wave / Continue | `Generated_GameplayHudPanel/WaveActionPanel/StartWaveButton` | No | No existing view event was found for this action. Leave visual-only unless a presenter-owned event is added later. |");
        builder.AppendLine("| Restart | `Generated_ResultPopupSkin/Body/Buttons/RestartButton` | No | No existing restart view event was found. Do not wire this directly to gameplay from the generated prefab. |");
        builder.AppendLine("| Return to Menu | `Generated_ResultPopupSkin/Body/Buttons/ReturnToMenuButton` | No | Current result flow returns to menu through `DefendResultPresenter.OnPopupClosed`; use the existing popup close event rather than direct gameplay wiring. |");
        builder.AppendLine("| Result OK/Close | `Generated_ResultPopupSkin/Body/Buttons/OkButton` and `Generated_ResultPopupSkin/Body/CloseButton` | No | If this skin replaces the message popup body, assign the chosen close button to `MessagePopupView._okButton`; `MessagePopupView.OnOkClicked` already calls the popup close request. |");
        builder.AppendLine();
        builder.AppendLine("Run `Tools -> Defend -> Validate Active UI Setup` after assigning generated skin instances in the Inspector.");

        Directory.CreateDirectory(Path.GetDirectoryName(_buttonReportPath));
        File.WriteAllText(_buttonReportPath, builder.ToString());
        Debug.Log($"Button auto-wiring report written to {_buttonReportPath}.");
    }

    private static List<T> FindSceneObjects<T>(Scene scene)
        where T : MonoBehaviour
    {
        T[] allObjects = Resources.FindObjectsOfTypeAll<T>();
        List<T> sceneObjects = new List<T>();

        for (int i = 0; i < allObjects.Length; i++)
        {
            T item = allObjects[i];

            if (item == null || item.gameObject == null)
            {
                continue;
            }

            if (EditorUtility.IsPersistent(item.gameObject))
            {
                continue;
            }

            if (item.gameObject.scene != scene)
            {
                continue;
            }

            sceneObjects.Add(item);
        }

        return sceneObjects;
    }

    private static void LogComponentCount(string componentName, int count)
    {
        if (count == 0)
        {
            Debug.LogWarning($"{componentName}: not found in active scene.");
            return;
        }

        Debug.Log($"{componentName}: found {count} instance(s) in active scene.");
    }

    private static void ValidateComponents<T>(List<T> components)
        where T : MonoBehaviour
    {
        for (int i = 0; i < components.Count; i++)
        {
            ValidateSerializedObjectReferences(components[i]);
            ValidateOptionalGeneratedSkinFields(components[i]);
        }
    }

    private static void ValidateSerializedObjectReferences(MonoBehaviour component)
    {
        SerializedObject serializedObject = new SerializedObject(component);
        SerializedProperty property = serializedObject.GetIterator();
        bool enterChildren = true;

        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (property.name == "m_Script" || IsOptionalGeneratedSkinField(component, property.name))
            {
                continue;
            }

            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                continue;
            }

            if (property.objectReferenceValue == null)
            {
                Debug.LogError($"{component.gameObject.name}: {component.GetType().Name} missing serialized reference '{property.propertyPath}'.", component);
            }
        }
    }

    private static void ValidateOptionalGeneratedSkinFields(MonoBehaviour component)
    {
        SerializedObject serializedObject = new SerializedObject(component);
        SerializedProperty property = serializedObject.GetIterator();
        bool enterChildren = true;

        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (IsOptionalGeneratedSkinField(component, property.name) == false)
            {
                continue;
            }

            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                continue;
            }

            if (property.objectReferenceValue == null)
            {
                Debug.LogWarning($"{component.gameObject.name}: optional generated skin field '{property.propertyPath}' is not assigned. Gameplay continues without it.", component);
            }
            else
            {
                Debug.Log($"{component.gameObject.name}: optional generated skin field '{property.propertyPath}' is assigned to '{property.objectReferenceValue.name}'.", component);
            }
        }
    }

    private static bool IsOptionalGeneratedSkinField(MonoBehaviour component, string fieldName)
    {
        if (fieldName == "_iconImage" &&
            (component is CurrencyRowView || component is MessagePopupView))
        {
            return false;
        }

        return _optionalGeneratedSkinFields.Contains(fieldName);
    }

    private static void ValidateViewButtonFields<T>(List<T> views, string fieldName, string label)
        where T : MonoBehaviour
    {
        for (int i = 0; i < views.Count; i++)
        {
            ValidateViewButtonField(views[i], fieldName, label);
        }
    }

    private static void ValidateViewButtonField(MonoBehaviour view, string fieldName, string label)
    {
        SerializedObject serializedObject = new SerializedObject(view);
        SerializedProperty property = serializedObject.FindProperty(fieldName);

        if (property == null)
        {
            Debug.LogWarning($"{view.GetType().Name}: field '{fieldName}' was not found while validating {label}.");
            return;
        }

        Button button = property.objectReferenceValue as Button;

        if (button == null)
        {
            Debug.LogError($"{view.gameObject.name}: {label} button field '{fieldName}' is not assigned.", view);
            return;
        }

        Debug.Log($"{view.gameObject.name}: {label} button is assigned to '{button.gameObject.name}'. Existing view code wires it through named handlers at runtime.", button);
    }

    private static void ValidateNamedSceneButtons(Scene scene, string label, string[] keywords)
    {
        List<Button> buttons = FindSceneObjects<Button>(scene);
        int found = 0;

        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            string name = Normalize(button.gameObject.name);

            if (ContainsAnyKeyword(name, keywords) == false)
            {
                continue;
            }

            found++;
            Debug.Log($"{label}: found scene button '{GetHierarchyPath(button.transform)}'. Verify it connects through an existing view event before use.", button);
        }

        if (found == 0)
        {
            Debug.LogWarning($"{label}: no matching button was found in the active scene. This is okay if the current UI does not expose that action.");
        }
    }

    private static bool ContainsAnyKeyword(string value, string[] keywords)
    {
        for (int i = 0; i < keywords.Length; i++)
        {
            if (MatchesKeyword(value, Normalize(keywords[i])))
            {
                return true;
            }
        }

        return false;
    }

    private static string GetHierarchyPath(Transform transform)
    {
        StringBuilder builder = new StringBuilder(transform.name);
        Transform current = transform.parent;

        while (current != null)
        {
            builder.Insert(0, current.name + "/");
            current = current.parent;
        }

        return builder.ToString();
    }

    private sealed class AssetCatalog
    {
        public readonly List<AssetCandidate> Sprites = new List<AssetCandidate>();
        public readonly List<AssetCandidate> Prefabs = new List<AssetCandidate>();
        public readonly List<AssetCandidate> TmpFonts = new List<AssetCandidate>();
        public readonly List<AssetCandidate> Fonts = new List<AssetCandidate>();
    }

    private sealed class AssetCandidate
    {
        public UnityEngine.Object Asset;
        public string Name;
        public string Path;
    }

    private sealed class MatchedAsset
    {
        public UnityEngine.Object Asset;
        public string Keyword;
        public string Path;
        public int Score;
        public string Usage;
    }

    private sealed class CandidateSelection
    {
        private readonly Dictionary<string, MatchedAsset> _matches = new Dictionary<string, MatchedAsset>();

        public void Add(string key, MatchedAsset match)
        {
            if (match == null)
            {
                return;
            }

            _matches[key] = match;
        }

        public Sprite Sprite(string key)
        {
            MatchedAsset match;

            if (_matches.TryGetValue(key, out match))
            {
                return match.Asset as Sprite;
            }

            return null;
        }

        public TMP_FontAsset TmpFont(string key)
        {
            MatchedAsset match;

            if (_matches.TryGetValue(key, out match))
            {
                return match.Asset as TMP_FontAsset;
            }

            return null;
        }

        public List<MatchedAsset> GetMatches()
        {
            return new List<MatchedAsset>(_matches.Values);
        }
    }
}
