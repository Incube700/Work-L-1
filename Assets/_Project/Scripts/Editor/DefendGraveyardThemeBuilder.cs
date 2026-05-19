using System.IO;
using UnityEditor;
using UnityEngine;

public static class DefendGraveyardThemeBuilder
{
    private const string _generatedRoot = "Assets/_LocalGenerated/GraveyardDefense";
    private const string _resourcesRoot = _generatedRoot + "/Resources";
    private const string _entitiesRoot = _resourcesRoot + "/Entities/GraveyardDefense";
    private const string _projectilesRoot = _resourcesRoot + "/Prefabs/GraveyardDefense";
    private const string _configsRoot = _resourcesRoot + "/Configs/DefendGame/GraveyardDefense";
    private const string _partsRoot = _configsRoot + "/Parts";
    private const string _wavesRoot = _configsRoot + "/Waves";

    private const string _dummyEnemyPath = "Assets/_Project/Resources/Entities/DummyEnemy.prefab";
    private const string _dummyShooterEnemyPath = "Assets/_Project/Resources/Entities/DummyEnemyShooter.prefab";
    private const string _dummyBuildingPath = "Assets/_Project/Resources/Entities/DummyBuilding.prefab";
    private const string _dummyMinePath = "Assets/_Project/Resources/Entities/DummyMine.prefab";
    private const string _dummyTurretPath = "Assets/_Project/Resources/Entities/DummyTurret.prefab";
    private const string _dummyPuddlePath = "Assets/_Project/Resources/Entities/DummyPuddle.prefab";
    private const string _dummyProjectilePath = "Assets/_Project/Resources/Prefabs/ProjectileFireball.prefab";

    private const string _enemyConfigPath = "Assets/_Project/Resources/Configs/DefendGame/Parts/EnemyConfig.asset";
    private const string _shooterEnemyConfigPath = "Assets/_Project/Resources/Configs/DefendGame/Parts/ShooterEnemyConfig.asset";
    private const string _buildingConfigPath = "Assets/_Project/Resources/Configs/DefendGame/Parts/BuildingConfig.asset";
    private const string _mineConfigPath = "Assets/_Project/Resources/Configs/DefendGame/Parts/MineConfig.asset";
    private const string _turretConfigPath = "Assets/_Project/Resources/Configs/DefendGame/Parts/TurretConfig.asset";
    private const string _puddleConfigPath = "Assets/_Project/Resources/Configs/DefendGame/Parts/PuddleConfig.asset";
    private const string _projectileConfigPath = "Assets/_Project/Resources/Configs/DefendGame/ProjectileConfig.asset";
    private const string _levelConfigPath = "Assets/_Project/Resources/Configs/DefendGame/Levels/Level_01.asset";

    private const string _zombieVisualPath = "Assets/Toon_Zombies_extended/prefabs/male_civilians/TZ_M_Civ_01.prefab";
    private const string _rangedUndeadVisualPath = "Assets/Toon_Zombies_extended/prefabs/specials/TZ_Police_01.prefab";
    private const string _heavyZombieVisualPath = "Assets/Toon_Zombies_extended/prefabs/specials/TZ_Tank_01.prefab";
    private const string _runeVfxPath = "Assets/MasterMagicFX/Particles/Field_Purple/Prefabs/Par_PurpleField.prefab";
    private const string _totemVfxPath = "Assets/MasterMagicFX/Particles/Shoot_Purple/Prefabs/Par_PurpleShoot_Muzzle.prefab";
    private const string _projectileVfxPath = "Assets/MasterMagicFX/Particles/Shoot_Purple/Prefabs/Par_PurpleShoot_Bullet.prefab";
    private const string _curseVfxPath = "Assets/MasterMagicFX/Particles/Buff_Debuff/Prefabs/Par_Debuff.prefab";
    private const string _baseVfxPath = "Assets/MasterMagicFX/Particles/Shield_Purple/Prefabs/Par_PurpleShield.prefab";
    private const string _graveMarkerPath = "Assets/3DShapes/Colonial City LittlePack – Church Graveyard Environment/Prefabs/Graveyard/Grave14.prefab";
    private const string _totemMarkerPath = "Assets/3DShapes/Colonial City LittlePack – Church Graveyard Environment/Prefabs/Graveyard/StreetLamp1.prefab";

    [MenuItem("Tools/Defend/Build Graveyard Defense Theme (Local)")]
    public static void BuildLocalTheme()
    {
        EnsureGeneratedFolders();

        ThemeAssets assets = LocateThemeAssets();

        CreateEntityWrapper(
            "ZombieEnemyView",
            _dummyEnemyPath,
            assets.ZombieVisual,
            Vector3.zero,
            Vector3.one,
            "Entities/GraveyardDefense/ZombieEnemyView");

        CreateEntityWrapper(
            "RangedUndeadEnemyView",
            _dummyShooterEnemyPath,
            assets.RangedUndeadVisual,
            Vector3.zero,
            Vector3.one,
            "Entities/GraveyardDefense/RangedUndeadEnemyView");

        CreateEntityWrapper(
            "HeavyZombieEnemyView",
            _dummyEnemyPath,
            assets.HeavyZombieVisual,
            Vector3.zero,
            new Vector3(1.15f, 1.15f, 1.15f),
            "Entities/GraveyardDefense/HeavyZombieEnemyView");

        CreateEntityWrapper(
            "MagicRuneMineView",
            _dummyMinePath,
            assets.RuneVfx,
            new Vector3(0f, 0.08f, 0f),
            new Vector3(0.65f, 0.65f, 0.65f),
            "Entities/GraveyardDefense/MagicRuneMineView");

        CreateEntityWrapper(
            "MagicTotemTurretView",
            _dummyTurretPath,
            assets.TotemMarker,
            Vector3.zero,
            new Vector3(0.65f, 0.65f, 0.65f),
            "Entities/GraveyardDefense/MagicTotemTurretView",
            assets.TotemVfx);

        CreateEntityWrapper(
            "PoisonSwampPuddleView",
            _dummyPuddlePath,
            assets.CurseVfx,
            new Vector3(0f, 0.08f, 0f),
            new Vector3(0.75f, 0.75f, 0.75f),
            "Entities/GraveyardDefense/PoisonSwampPuddleView");

        CreateEntityWrapper(
            "RitualStoneBaseView",
            _dummyBuildingPath,
            assets.GraveMarker,
            Vector3.zero,
            new Vector3(1.2f, 1.2f, 1.2f),
            "Entities/GraveyardDefense/RitualStoneBaseView",
            assets.BaseVfx);

        CreateEntityWrapper(
            "PurpleBoltProjectileView",
            _dummyProjectilePath,
            assets.ProjectileVfx,
            Vector3.zero,
            Vector3.one,
            "Prefabs/GraveyardDefense/PurpleBoltProjectileView",
            outputFolder: _projectilesRoot);

        CreateConfigAssets();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "Local Graveyard Defense theme generated under Assets/_LocalGenerated/GraveyardDefense. " +
            "These assets are ignored and must not be committed.");
        Debug.Log(
            "Manual activation: use the generated GraveyardDefense_Level_01 config for local testing only, " +
            "or temporarily add it to DefendLevelsConfig and revert that tracked config before committing.");
    }

    private static void EnsureGeneratedFolders()
    {
        EnsureFolder("Assets/_LocalGenerated");
        EnsureFolder(_generatedRoot);
        EnsureFolder(_resourcesRoot);
        EnsureFolder(_entitiesRoot);
        EnsureFolder(_projectilesRoot);
        EnsureFolder(_configsRoot);
        EnsureFolder(_partsRoot);
        EnsureFolder(_wavesRoot);
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
            Debug.LogError($"Cannot create folder '{assetFolder}'.");
            return;
        }

        parent = parent.Replace("\\", "/");
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, Path.GetFileName(assetFolder));
    }

    private static ThemeAssets LocateThemeAssets()
    {
        ThemeAssets assets = new ThemeAssets
        {
            ZombieVisual = LoadOptional<GameObject>(_zombieVisualPath, "zombie visual"),
            RangedUndeadVisual = LoadOptional<GameObject>(_rangedUndeadVisualPath, "ranged undead visual"),
            HeavyZombieVisual = LoadOptional<GameObject>(_heavyZombieVisualPath, "heavy zombie visual"),
            RuneVfx = LoadOptional<GameObject>(_runeVfxPath, "magic rune VFX"),
            TotemVfx = LoadOptional<GameObject>(_totemVfxPath, "totem muzzle VFX"),
            ProjectileVfx = LoadOptional<GameObject>(_projectileVfxPath, "purple bolt projectile VFX"),
            CurseVfx = LoadOptional<GameObject>(_curseVfxPath, "poison curse VFX"),
            BaseVfx = LoadOptional<GameObject>(_baseVfxPath, "ritual stone shield VFX"),
            GraveMarker = LoadOptional<GameObject>(_graveMarkerPath, "grave marker prop"),
            TotemMarker = LoadOptional<GameObject>(_totemMarkerPath, "totem marker prop")
        };

        return assets;
    }

    private static T LoadOptional<T>(string assetPath, string usage)
        where T : UnityEngine.Object
    {
        T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

        if (asset == null)
        {
            Debug.LogWarning($"Local Graveyard theme candidate missing for {usage}: {assetPath}");
        }
        else
        {
            Debug.Log($"Local Graveyard theme candidate selected for {usage}: {assetPath}");
        }

        return asset;
    }

    private static void CreateEntityWrapper(
        string prefabName,
        string basePrefabPath,
        GameObject primaryVisual,
        Vector3 visualPosition,
        Vector3 visualScale,
        string resourcesPath,
        GameObject secondaryVisual = null,
        string outputFolder = _entitiesRoot)
    {
        string outputPath = outputFolder + "/" + prefabName + ".prefab";

        if (File.Exists(outputPath) && ConfirmReplace(outputPath) == false)
        {
            Debug.LogWarning($"Skipped existing local Graveyard prefab: {outputPath}");
            return;
        }

        GameObject basePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(basePrefabPath);

        if (basePrefab == null)
        {
            Debug.LogError($"Cannot create {prefabName}. Base prefab missing: {basePrefabPath}");
            return;
        }

        GameObject instance = PrefabUtility.InstantiatePrefab(basePrefab) as GameObject;

        if (instance == null)
        {
            instance = UnityEngine.Object.Instantiate(basePrefab);
        }

        try
        {
            instance.name = prefabName;
            HideExistingRenderers(instance);

            GameObject visualInstance = AddVisual(instance.transform, primaryVisual, "ThemeVisual", visualPosition, visualScale);
            AddVisual(instance.transform, secondaryVisual, "ThemeAccent", new Vector3(0f, 0.35f, 0f), Vector3.one);
            AssignEnemyAnimator(instance, visualInstance);

            PrefabUtility.SaveAsPrefabAsset(instance, outputPath, out bool success);

            if (success)
            {
                Debug.Log($"Generated local Graveyard prefab: {outputPath} | Resources path: {resourcesPath}");
            }
            else
            {
                Debug.LogError($"Failed to save local Graveyard prefab: {outputPath}");
            }
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(instance);
        }
    }

    private static GameObject AddVisual(
        Transform parent,
        GameObject visualPrefab,
        string childName,
        Vector3 localPosition,
        Vector3 localScale)
    {
        if (visualPrefab == null)
        {
            return CreateFallbackVisual(parent, childName, localPosition, localScale);
        }

        GameObject visual = PrefabUtility.InstantiatePrefab(visualPrefab) as GameObject;

        if (visual == null)
        {
            visual = UnityEngine.Object.Instantiate(visualPrefab);
        }

        visual.name = childName + "_" + visualPrefab.name;
        visual.transform.SetParent(parent, false);
        visual.transform.localPosition = localPosition;
        visual.transform.localRotation = Quaternion.identity;
        visual.transform.localScale = localScale;

        StripVisualGameplayComponents(visual);
        return visual;
    }

    private static GameObject CreateFallbackVisual(
        Transform parent,
        string childName,
        Vector3 localPosition,
        Vector3 localScale)
    {
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        visual.name = childName + "_Fallback";
        visual.transform.SetParent(parent, false);
        visual.transform.localPosition = localPosition;
        visual.transform.localRotation = Quaternion.identity;
        visual.transform.localScale = localScale;

        Collider collider = visual.GetComponent<Collider>();

        if (collider != null)
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }

        return visual;
    }

    private static void HideExistingRenderers(GameObject root)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }
    }

    private static void StripVisualGameplayComponents(GameObject visual)
    {
        Collider[] colliders = visual.GetComponentsInChildren<Collider>(true);

        for (int i = 0; i < colliders.Length; i++)
        {
            UnityEngine.Object.DestroyImmediate(colliders[i]);
        }

        Rigidbody[] rigidbodies = visual.GetComponentsInChildren<Rigidbody>(true);

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            UnityEngine.Object.DestroyImmediate(rigidbodies[i]);
        }

        MonoBehaviour[] behaviours = visual.GetComponentsInChildren<MonoBehaviour>(true);

        for (int i = 0; i < behaviours.Length; i++)
        {
            UnityEngine.Object.DestroyImmediate(behaviours[i]);
        }
    }

    private static void AssignEnemyAnimator(GameObject root, GameObject visual)
    {
        if (visual == null)
        {
            return;
        }

        EnemyAnimatorView animatorView = root.GetComponentInChildren<EnemyAnimatorView>(true);
        Animator animator = visual.GetComponentInChildren<Animator>(true);

        if (animatorView == null || animator == null)
        {
            return;
        }

        SerializedObject serializedObject = new SerializedObject(animatorView);
        SerializedProperty animatorProperty = serializedObject.FindProperty("_animator");

        if (animatorProperty != null)
        {
            animatorProperty.objectReferenceValue = animator;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }

    private static void CreateConfigAssets()
    {
        EnemyConfig zombieConfig = CopyConfig<EnemyConfig>(
            _enemyConfigPath,
            _partsRoot + "/Graveyard_ZombieEnemyConfig.asset");
        SetCommonEnemyFields(zombieConfig, 42f, 2.4f, "Entities/GraveyardDefense/ZombieEnemyView");
        SetFloat(zombieConfig, "_explodeDistance", 1.2f);
        SetFloat(zombieConfig, "_explodeDamage", 22f);

        ProjectileConfig projectileConfig = CopyConfig<ProjectileConfig>(
            _projectileConfigPath,
            _partsRoot + "/Graveyard_PurpleBoltProjectileConfig.asset");
        SetString(projectileConfig, "_prefabPath", "Prefabs/GraveyardDefense/PurpleBoltProjectileView");

        ShooterEnemyConfig rangedConfig = CopyConfig<ShooterEnemyConfig>(
            _shooterEnemyConfigPath,
            _partsRoot + "/Graveyard_RangedUndeadEnemyConfig.asset");
        SetCommonEnemyFields(rangedConfig, 34f, 2.1f, "Entities/GraveyardDefense/RangedUndeadEnemyView");
        SetFloat(rangedConfig, "_attackDistance", 5.5f);
        SetFloat(rangedConfig, "_attackInterval", 1.8f);
        SetObject(rangedConfig, "_projectileConfig", projectileConfig);

        EnemyConfig heavyConfig = CopyConfig<EnemyConfig>(
            _enemyConfigPath,
            _partsRoot + "/Graveyard_HeavyZombieEnemyConfig.asset");
        SetCommonEnemyFields(heavyConfig, 95f, 1.35f, "Entities/GraveyardDefense/HeavyZombieEnemyView");
        SetFloat(heavyConfig, "_explodeDistance", 1.45f);
        SetFloat(heavyConfig, "_explodeDamage", 35f);

        BuildingConfig buildingConfig = CopyConfig<BuildingConfig>(
            _buildingConfigPath,
            _partsRoot + "/Graveyard_RitualStoneBuildingConfig.asset");
        SetFloat(buildingConfig, "_health", 220f);
        SetString(buildingConfig, "_prefabPath", "Entities/GraveyardDefense/RitualStoneBaseView");

        MineConfig mineConfig = CopyConfig<MineConfig>(
            _mineConfigPath,
            _partsRoot + "/Graveyard_MagicRuneConfig.asset");
        SetInt(mineConfig, "_costGold", 12);
        SetString(mineConfig, "_prefabPath", "Entities/GraveyardDefense/MagicRuneMineView");

        TurretConfig turretConfig = CopyConfig<TurretConfig>(
            _turretConfigPath,
            _partsRoot + "/Graveyard_MagicTotemConfig.asset");
        SetInt(turretConfig, "_costGold", 28);
        SetString(turretConfig, "_prefabPath", "Entities/GraveyardDefense/MagicTotemTurretView");
        SetObject(turretConfig, "_projectileConfig", projectileConfig);

        PuddleConfig puddleConfig = CopyConfig<PuddleConfig>(
            _puddleConfigPath,
            _partsRoot + "/Graveyard_PoisonSwampConfig.asset");
        SetInt(puddleConfig, "_costGold", 18);
        SetString(puddleConfig, "_prefabPath", "Entities/GraveyardDefense/PoisonSwampPuddleView");

        WaveConfig wave1 = CreateWave("Graveyard_Horde_01.asset", zombieConfig, 5, 0.75f, 10f);
        WaveConfig wave2 = CreateWave("Graveyard_Horde_02.asset", zombieConfig, 8, 0.65f, 11f);
        WaveConfig wave3 = CreateWave("Graveyard_Horde_03.asset", rangedConfig, 4, 1.2f, 12f);
        WaveConfig wave4 = CreateWave("Graveyard_Horde_04.asset", heavyConfig, 2, 1.4f, 12f);

        DefendLevelConfig levelConfig = CopyConfig<DefendLevelConfig>(
            _levelConfigPath,
            _configsRoot + "/GraveyardDefense_Level_01.asset");
        SetInt(levelConfig, "_winRewardGold", 30);
        SetInt(levelConfig, "_winRewardDiamonds", 2);
        SetFloat(levelConfig, "_restDurationSeconds", 7f);
        SetObject(levelConfig, "_buildingConfig", buildingConfig);
        SetObject(levelConfig, "_mineConfig", mineConfig);
        SetObject(levelConfig, "_turretConfig", turretConfig);
        SetObject(levelConfig, "_puddleConfig", puddleConfig);
        SetObjectArray(levelConfig, "_waves", wave1, wave2, wave3, wave4);

        DefendLevelsConfig levelsConfig = CreateOrReplace<DefendLevelsConfig>(
            _configsRoot + "/GraveyardDefenseLevelsConfig.asset");
        SetObjectArray(levelsConfig, "_levels", levelConfig);

        DefendUiIconConfig iconConfig = CreateOrReplace<DefendUiIconConfig>(
            _configsRoot + "/GraveyardDefenseUiIconConfig.asset");
        SetGraveyardIconLabels(iconConfig);

        DefendPresentationFeedbackConfig feedbackConfig = CreateOrReplace<DefendPresentationFeedbackConfig>(
            _configsRoot + "/GraveyardDefenseFeedbackConfig.asset");
        SetFeedbackVfx(feedbackConfig);

        Debug.Log($"Generated local Graveyard level config: {AssetDatabase.GetAssetPath(levelConfig)}");
        Debug.Log($"Projectile config copied for future purple bolt setup: {AssetDatabase.GetAssetPath(projectileConfig)}");
    }

    private static WaveConfig CreateWave(
        string fileName,
        EnemyConfigBase enemyConfig,
        int enemiesCount,
        float spawnInterval,
        float spawnRadius)
    {
        WaveConfig wave = CreateOrReplace<WaveConfig>(_wavesRoot + "/" + fileName);
        SetObject(wave, "_enemyConfig", enemyConfig);
        SetInt(wave, "_enemiesCount", enemiesCount);
        SetFloat(wave, "_spawnInterval", spawnInterval);
        SetFloat(wave, "_spawnRadius", spawnRadius);
        return wave;
    }

    private static T CopyConfig<T>(string sourcePath, string outputPath)
        where T : ScriptableObject
    {
        T existing = AssetDatabase.LoadAssetAtPath<T>(outputPath);

        if (existing != null)
        {
            if (ConfirmReplace(outputPath) == false)
            {
                Debug.LogWarning($"Using existing local config without replacing: {outputPath}");
                return existing;
            }

            AssetDatabase.DeleteAsset(outputPath);
        }

        T source = AssetDatabase.LoadAssetAtPath<T>(sourcePath);

        if (source == null)
        {
            Debug.LogWarning($"Source config missing, creating empty {typeof(T).Name}: {sourcePath}");
            return CreateOrReplace<T>(outputPath);
        }

        T copy = UnityEngine.Object.Instantiate(source);
        copy.name = Path.GetFileNameWithoutExtension(outputPath);
        AssetDatabase.CreateAsset(copy, outputPath);
        return copy;
    }

    private static T CreateOrReplace<T>(string outputPath)
        where T : ScriptableObject
    {
        T existing = AssetDatabase.LoadAssetAtPath<T>(outputPath);

        if (existing != null)
        {
            if (ConfirmReplace(outputPath) == false)
            {
                Debug.LogWarning($"Using existing local config without replacing: {outputPath}");
                return existing;
            }

            AssetDatabase.DeleteAsset(outputPath);
        }

        T asset = ScriptableObject.CreateInstance<T>();
        asset.name = Path.GetFileNameWithoutExtension(outputPath);
        AssetDatabase.CreateAsset(asset, outputPath);
        return asset;
    }

    private static void SetCommonEnemyFields(EnemyConfigBase config, float health, float moveSpeed, string prefabPath)
    {
        SetFloat(config, "_health", health);
        SetFloat(config, "_moveSpeed", moveSpeed);
        SetString(config, "_prefabPath", prefabPath);
    }

    private static void SetGraveyardIconLabels(DefendUiIconConfig config)
    {
        SerializedObject serializedObject = new SerializedObject(config);
        SetPresentationEntry(serializedObject, "_currencyEntries", 0, "_type", (int)CurrencyType.Gold, "Souls", null);
        SetPresentationEntry(serializedObject, "_currencyEntries", 1, "_type", (int)CurrencyType.Diamond, "Crystals", null);
        SetPresentationEntry(serializedObject, "_placeableEntries", 0, "_type", (int)PlaceableType.Mine, "Rune", null);
        SetPresentationEntry(serializedObject, "_placeableEntries", 1, "_type", (int)PlaceableType.Turret, "Totem", null);
        SetPresentationEntry(serializedObject, "_placeableEntries", 2, "_type", (int)PlaceableType.Puddle, "Curse", null);
        SetPresentationEntry(serializedObject, "_phaseEntries", 0, "_phase", (int)DefendPhase.Wave, "Defend", null);
        SetPresentationEntry(serializedObject, "_phaseEntries", 1, "_phase", (int)DefendPhase.Rest, "Prepare", null);
        SetPresentationEntry(serializedObject, "_phaseEntries", 2, "_phase", (int)DefendPhase.Ended, "Result", null);
        SetArraySize(serializedObject, "_enemyEntries", 0);
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(config);
    }

    private static void SetPresentationEntry(
        SerializedObject serializedObject,
        string arrayName,
        int index,
        string enumField,
        int enumValue,
        string displayName,
        Sprite icon)
    {
        SerializedProperty array = serializedObject.FindProperty(arrayName);

        if (array == null)
        {
            return;
        }

        if (array.arraySize <= index)
        {
            array.arraySize = index + 1;
        }

        SerializedProperty entry = array.GetArrayElementAtIndex(index);
        SetEnumProperty(entry.FindPropertyRelative(enumField), enumValue);
        entry.FindPropertyRelative("_displayName").stringValue = displayName;
        entry.FindPropertyRelative("_icon").objectReferenceValue = icon;
    }

    private static void SetFeedbackVfx(DefendPresentationFeedbackConfig config)
    {
        SerializedObject serializedObject = new SerializedObject(config);
        SerializedProperty sfxCues = serializedObject.FindProperty("_sfxCues");

        if (sfxCues != null)
        {
            sfxCues.arraySize = 0;
        }

        SerializedProperty vfxCues = serializedObject.FindProperty("_vfxCues");

        if (vfxCues != null)
        {
            vfxCues.arraySize = 4;
            SetVfxEntry(vfxCues, 0, DefendVfxCue.PlacementPreview, _runeVfxPath);
            SetVfxEntry(vfxCues, 1, DefendVfxCue.PlaceableConfirmed, _runeVfxPath);
            SetVfxEntry(vfxCues, 2, DefendVfxCue.TowerAttack, _totemVfxPath);
            SetVfxEntry(vfxCues, 3, DefendVfxCue.EnemyDeath, _curseVfxPath);
        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(config);
    }

    private static void SetVfxEntry(SerializedProperty array, int index, DefendVfxCue cue, string prefabPath)
    {
        SerializedProperty entry = array.GetArrayElementAtIndex(index);
        SetEnumProperty(entry.FindPropertyRelative("_cue"), (int)cue);
        entry.FindPropertyRelative("_prefab").objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    }

    private static void SetArraySize(SerializedObject serializedObject, string propertyName, int size)
    {
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property != null && property.isArray)
        {
            property.arraySize = size;
        }
    }

    private static void SetFloat(UnityEngine.Object target, string propertyName, float value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property != null)
        {
            property.floatValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }
    }

    private static void SetInt(UnityEngine.Object target, string propertyName, int value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property != null)
        {
            property.intValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }
    }

    private static void SetString(UnityEngine.Object target, string propertyName, string value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property != null)
        {
            property.stringValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }
    }

    private static void SetObject(UnityEngine.Object target, string propertyName, UnityEngine.Object value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property != null)
        {
            property.objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }
    }

    private static void SetObjectArray(UnityEngine.Object target, string propertyName, params UnityEngine.Object[] values)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property == null || property.isArray == false)
        {
            return;
        }

        property.arraySize = values.Length;

        for (int i = 0; i < values.Length; i++)
        {
            property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
    }

    private static void SetEnumProperty(SerializedProperty property, int value)
    {
        if (property == null)
        {
            return;
        }

        if (property.propertyType == SerializedPropertyType.Enum)
        {
            property.enumValueIndex = Mathf.Clamp(value, 0, property.enumDisplayNames.Length - 1);
        }
        else
        {
            property.intValue = value;
        }
    }

    private static bool ConfirmReplace(string path)
    {
        return EditorUtility.DisplayDialog(
            "Replace Local Graveyard Asset",
            $"{path} already exists. Replace this ignored local generated asset?",
            "Replace",
            "Skip");
    }

    private sealed class ThemeAssets
    {
        public GameObject ZombieVisual;
        public GameObject RangedUndeadVisual;
        public GameObject HeavyZombieVisual;
        public GameObject RuneVfx;
        public GameObject TotemVfx;
        public GameObject ProjectileVfx;
        public GameObject CurseVfx;
        public GameObject BaseVfx;
        public GameObject GraveMarker;
        public GameObject TotemMarker;
    }
}
