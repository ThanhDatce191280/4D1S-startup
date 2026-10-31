using System.Collections.Generic;
using System.IO;
using Cinemachine;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Core;
using HuyetNguyetDaiNgan.Inventory;
using HuyetNguyetDaiNgan.Player;
using HuyetNguyetDaiNgan.UI;
using HuyetNguyetDaiNgan.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HuyetNguyetDaiNgan.Editor
{
    public static class M5_InventorySceneSetupHelper
    {
        private const string ScenePath = "Assets/_Project/Scenes/Sandbox/Test_InventoryCrafting.unity";
        private const string DataFolder = "Assets/_Project/Data";
        private const string ItemsFolder = DataFolder + "/Items";
        private const string RecipesFolder = DataFolder + "/Recipes";
        private const string ConfigFolder = DataFolder + "/Config";
        private const string MaterialsFolder = "Assets/_Project/Art/Materials";

        [MenuItem("Huyết Nguyệt Đại Ngàn/M5: Setup Inventory & Crafting Scene", false, 60)]
        public static void SetupM5InventoryScene()
        {
            if (File.Exists(ScenePath))
            {
                Debug.LogWarning($"[Huyết Nguyệt M5] Scene '{ScenePath}' đã tồn tại. Helper dừng để không ghi đè scene hoặc prefab người dùng. Hãy đổi tên/xóa scene có chủ đích trước khi tạo mới.");
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
                return;
            }

            EnsureDirectories();

            Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLit == null) urpLit = Shader.Find("Standard");

            // 1. Materials
            Material grassMat = GetOrCreateMaterial(MaterialsFolder + "/M_Survival_Grass.mat", new Color(0.28f, 0.42f, 0.22f, 1f), urpLit);
            Material trunkMat = GetOrCreateMaterial(MaterialsFolder + "/M_Wood_Trunk.mat", new Color(0.45f, 0.28f, 0.15f, 1f), urpLit);
            Material foliageMat = GetOrCreateMaterial(MaterialsFolder + "/M_Wood_Foliage.mat", new Color(0.18f, 0.48f, 0.22f, 1f), urpLit);
            Material stoneMat = GetOrCreateMaterial(MaterialsFolder + "/M_Stone_Rock.mat", new Color(0.55f, 0.58f, 0.62f, 1f), urpLit);
            Material bushMat = GetOrCreateMaterial(MaterialsFolder + "/M_Berry_Bush.mat", new Color(0.25f, 0.55f, 0.25f, 1f), urpLit);
            Material berryMat = GetOrCreateMaterial(MaterialsFolder + "/M_Berry_Fruit.mat", new Color(0.92f, 0.20f, 0.20f, 1f), urpLit);
            Material stumpMat = GetOrCreateMaterial(MaterialsFolder + "/M_Wood_Stump.mat", new Color(0.38f, 0.25f, 0.14f, 1f), urpLit);
            Material wallMat = GetOrCreateMaterial(MaterialsFolder + "/M_SurvivalWall_URP.mat", new Color(0.32f, 0.35f, 0.30f, 1f), urpLit);

            // 2. Tạo Item Database & Recipe Database
            var items = CreateOrLoadItemDatabase();
            var recipes = CreateOrLoadRecipeDatabase(items);

            SurvivalConfigSO survivalConfig = GetOrCreateSurvivalConfig();
            WeaponDataSO swordWeaponData = GetOrCreateWeaponData();

            // 3. New Scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 4. Lighting & Environment
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.40f, 0.42f, 0.45f, 1f);

            var lightObj = new GameObject("Directional Light (Sun)");
            var sunLight = lightObj.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.color = new Color(1f, 0.96f, 0.88f);
            sunLight.intensity = 1.35f;
            sunLight.shadows = LightShadows.Soft;
            lightObj.transform.rotation = Quaternion.Euler(50f, -35f, 0f);

            // 5. Arena Ground (50m x 50m)
            var groundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            groundObj.name = "Survival_Forest_Floor";
            groundObj.transform.position = Vector3.zero;
            groundObj.transform.localScale = new Vector3(5f, 1f, 5f);
            groundObj.GetComponent<Renderer>().sharedMaterial = grassMat;

            CreateBoundaryWalls(groundObj, wallMat);

            // 6. Spawn Resource Nodes liên kết với ItemDataSO
            GameObject environmentRoot = new GameObject("Environment_Resources");

            // Rừng cây (Rơi Gỗ Khúc)
            Vector3[] treePositions = new Vector3[]
            {
                new Vector3(-6f, 0f, 4f),
                new Vector3(-9f, 0f, 6f),
                new Vector3(-4f, 0f, 8f),
                new Vector3(7f, 0f, 5f),
                new Vector3(10f, 0f, 7f),
                new Vector3(5f, 0f, 10f),
                new Vector3(-12f, 0f, -5f)
            };
            for (int i = 0; i < treePositions.Length; i++)
            {
                CreateTreeResourceNode(treePositions[i], i + 1, trunkMat, foliageMat, stumpMat, items["resource_wood"], environmentRoot.transform);
            }

            // Mỏ đá (Rơi Đá Cuội)
            Vector3[] rockPositions = new Vector3[]
            {
                new Vector3(6f, 0f, -4f),
                new Vector3(9f, 0f, -7f),
                new Vector3(4f, 0f, -8f),
                new Vector3(12f, 0f, -3f),
                new Vector3(-7f, 0f, -10f)
            };
            for (int i = 0; i < rockPositions.Length; i++)
            {
                CreateRockResourceNode(rockPositions[i], i + 1, stoneMat, items["resource_stone"], environmentRoot.transform);
            }

            // Bụi quả dại (Rơi Quả Mọng)
            Vector3[] bushPositions = new Vector3[]
            {
                new Vector3(0f, 0f, 6f),
                new Vector3(-2f, 0f, -4f),
                new Vector3(2f, 0f, -5f),
                new Vector3(-5f, 0f, -2f),
                new Vector3(3f, 0f, 3f)
            };
            for (int i = 0; i < bushPositions.Length; i++)
            {
                CreateBerryBushNode(bushPositions[i], i + 1, bushMat, berryMat, items["resource_berry"], environmentRoot.transform);
            }

            // 7. Spawn Player with Inventory + Crafting + Survival
            GameObject player = CreatePlayerWithInventory(swordWeaponData, survivalConfig, items, recipes);
            player.transform.position = new Vector3(0f, 0.1f, 0f);

            // 8. Camera with Cinemachine
            var camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            var cam = camObj.AddComponent<Camera>();
            cam.fieldOfView = 40f;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 200f;
            camObj.AddComponent<UniversalAdditionalCameraData>();
            camObj.AddComponent<CinemachineBrain>();

            var vcamObj = new GameObject("CM_Inventory_Vcam");
            var vcam = vcamObj.AddComponent<CinemachineVirtualCamera>();
            vcam.Follow = player.transform;
            var transposer = vcam.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transposer.m_FollowOffset = new Vector3(-14f, 16f, -14f);
            transposer.m_XDamping = 0.5f;
            transposer.m_YDamping = 0.5f;
            transposer.m_ZDamping = 0.5f;
            vcam.transform.rotation = Quaternion.Euler(45f, 45f, 0f);

            // 9. UI Managers
            var hudObj = new GameObject("Survival_HUD");
            hudObj.AddComponent<SurvivalHUD>();

            var invUIObj = new GameObject("Inventory_Crafting_UI");
            invUIObj.AddComponent<InventoryCraftingUI>();

            // 10. Save Scene
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);

            AssetDatabase.Refresh();
            Debug.Log($"[Huyết Nguyệt M5] Đã thiết lập hoàn tất Scene '{ScenePath}' với Túi đồ 20 ô, Bàn chế tạo và 4 công thức sinh tồn!");
        }

        private static GameObject CreatePlayerWithInventory(WeaponDataSO weapon, SurvivalConfigSO config, Dictionary<string, ItemDataSO> items, List<CraftingRecipeSO> recipes)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/Player/Player_Prototype.prefab");
            GameObject player;

            if (prefab != null)
            {
                player = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                player = new GameObject("Player");
                var cc = player.AddComponent<CharacterController>();
                cc.height = 1.8f;
                cc.radius = 0.4f;
                cc.center = new Vector3(0f, 0.9f, 0f);
                player.AddComponent<PlayerInputHandler>();
                player.AddComponent<PlayerMovementController>();

                var vis = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                vis.name = "PlayerVisual";
                vis.transform.SetParent(player.transform);
                vis.transform.localPosition = new Vector3(0f, 0.9f, 0f);
                Object.DestroyImmediate(vis.GetComponent<Collider>());
            }

            player.name = "Player";
            player.tag = "Player";

            // Core Components
            var health = player.GetComponent<HealthComponent>();
            if (health == null) health = player.AddComponent<HealthComponent>();
            health.SetMaxHealth(100f, true);

            if (player.GetComponent<HitFeedbackFlash>() == null)
            {
                player.AddComponent<HitFeedbackFlash>();
            }

            var stats = player.GetComponent<PlayerStatsComponent>();
            if (stats == null) stats = player.AddComponent<PlayerStatsComponent>();
            stats.SetConfig(config);

            var combat = player.GetComponent<PlayerCombatController>();
            if (combat == null) combat = player.AddComponent<PlayerCombatController>();
            combat.EquipWeapon(weapon);

            // Inventory & Crafting Systems
            var inv = player.GetComponent<InventorySystem>();
            if (inv == null) inv = player.AddComponent<InventorySystem>();
            inv.InitializeSlots();

            var crafting = player.GetComponent<CraftingSystem>();
            if (crafting == null) crafting = player.AddComponent<CraftingSystem>();
            crafting.SetInventory(inv);
            crafting.SetRecipes(recipes);

            // Cho người chơi khởi đầu với vài nguyên liệu và cơm lam mẫu
            inv.TryAddItem(items["consumable_bamboo_rice"], 2);
            inv.TryAddItem(items["resource_wood"], 4);
            inv.TryAddItem(items["resource_stone"], 3);

            return player;
        }

        private static Dictionary<string, ItemDataSO> CreateOrLoadItemDatabase()
        {
            var dict = new Dictionary<string, ItemDataSO>();

            // 1. Gỗ Khúc
            dict["resource_wood"] = GetOrCreateItem("resource_wood", "Gỗ Khúc Rừng Già", "Gỗ đốn từ thân cây cổ thụ, nguyên liệu thô để nhóm lửa và chế tác công cụ.", ItemType.Resource, 99, "🪵");

            // 2. Đá Cuội
            dict["resource_stone"] = GetOrCreateItem("resource_stone", "Đá Cuội Sông Suối", "Tảng đá cứng cáp nhặt từ lòng suối, dùng ghè đẽo thành rìu cuốc.", ItemType.Resource, 99, "🪨");

            // 3. Quả Mọng
            var berry = GetOrCreateItem("resource_berry", "Quả Mọng Rừng Sâu", "Quả dại hái từ bụi rậm, vị ngọt thanh giúp giải tỏa cơn đói.", ItemType.Consumable, 50, "🍎");
            berry.hungerRestore = 15f;
            berry.healthRestore = 5f;
            dict["resource_berry"] = berry;

            // 4. Cơm Lam Ống Nứa
            var rice = GetOrCreateItem("consumable_bamboo_rice", "Cơm Lam Ống Nứa", "Món cơm lam nướng than hồng thơm lừng, hồi phục 45 Độ No và 30 Máu.", ItemType.Consumable, 20, "🍙");
            rice.hungerRestore = 45f;
            rice.healthRestore = 30f;
            dict["consumable_bamboo_rice"] = rice;

            // 5. Rìu Đá Thô Sơ
            var axe = GetOrCreateItem("tool_stone_axe_t1", "Rìu Đá Thô Sơ", "Lưỡi đá ghè buộc chặt vào cán gỗ, chặt cây nhanh gấp đôi tay không.", ItemType.Tool, 1, "🪓");
            dict["tool_stone_axe_t1"] = axe;

            // 6. Đuốc Sáng Đi Rừng
            var torch = GetOrCreateItem("tool_torch_t1", "Đuốc Sáng Đi Rừng", "Đuốc tẩm nhựa cây thắp sáng màn đêm và xua đuổi thú dữ ban đêm.", ItemType.Tool, 1, "🔥");
            dict["tool_torch_t1"] = torch;

            // 7. Trường Kiếm Sắt Ấp Làng
            WeaponDataSO swordWeaponData = GetOrCreateWeaponData();
            var sword = GetOrCreateItem("weapon_sword_iron_t1", "Trường Kiếm Sắt Ấp Làng", "Vũ khí rèn từ sắt non, sát thương 25 đòn đánh quét hình quạt.", ItemType.Weapon, 1, "⚔");
            sword.weaponData = swordWeaponData;
            dict["weapon_sword_iron_t1"] = sword;

            AssetDatabase.SaveAssets();
            return dict;
        }

        private static List<CraftingRecipeSO> CreateOrLoadRecipeDatabase(Dictionary<string, ItemDataSO> items)
        {
            var list = new List<CraftingRecipeSO>();

            // Recipe 1: Rìu Đá (3 Gỗ + 2 Đá)
            list.Add(GetOrCreateRecipe("recipe_stone_axe", "Rìu Đá Thô Sơ", "Ghè lưỡi đá buộc cán gỗ để tăng tốc độ khai thác cây rừng.",
                new (ItemDataSO, int)[] { (items["resource_wood"], 3), (items["resource_stone"], 2) },
                items["tool_stone_axe_t1"], 1));

            // Recipe 2: Đuốc Sáng (2 Gỗ + 1 Quả/Nhựa)
            list.Add(GetOrCreateRecipe("recipe_torch", "Đuốc Sáng Đi Rừng", "Bó cành gỗ bọc nhựa thắp sáng đường mòn ban đêm.",
                new (ItemDataSO, int)[] { (items["resource_wood"], 2), (items["resource_berry"], 1) },
                items["tool_torch_t1"], 1));

            // Recipe 3: Cơm Lam (2 Gỗ + 3 Quả Mọng)
            list.Add(GetOrCreateRecipe("recipe_bamboo_rice", "Cơm Lam Ống Nứa", "Đun nướng thức ăn giúp hồi phục sinh lực và độ no dồi dào.",
                new (ItemDataSO, int)[] { (items["resource_wood"], 2), (items["resource_berry"], 3) },
                items["consumable_bamboo_rice"], 1));

            // Recipe 4: Trường Kiếm Sắt (5 Gỗ + 5 Đá)
            list.Add(GetOrCreateRecipe("recipe_iron_sword", "Trường Kiếm Sắt", "Rèn luyện vũ khí tự vệ trước bầy sói khi đêm Huyết Nguyệt buông xuống.",
                new (ItemDataSO, int)[] { (items["resource_wood"], 5), (items["resource_stone"], 5) },
                items["weapon_sword_iron_t1"], 1));

            AssetDatabase.SaveAssets();
            return list;
        }

        private static ItemDataSO GetOrCreateItem(string id, string name, string desc, ItemType type, int maxStack, string glyph)
        {
            string path = $"{ItemsFolder}/Item_{id}.asset";
            ItemDataSO item = AssetDatabase.LoadAssetAtPath<ItemDataSO>(path);
            if (item == null)
            {
                item = ScriptableObject.CreateInstance<ItemDataSO>();
                item.itemId = id;
                item.itemName = name;
                item.description = desc;
                item.itemType = type;
                item.maxStackSize = maxStack;
                item.iconGlyph = glyph;

                AssetDatabase.CreateAsset(item, path);
            }
            return item;
        }

        private static CraftingRecipeSO GetOrCreateRecipe(string id, string name, string desc, (ItemDataSO item, int count)[] ingredients, ItemDataSO resultItem, int resultCount)
        {
            string path = $"{RecipesFolder}/Recipe_{id}.asset";
            CraftingRecipeSO recipe = AssetDatabase.LoadAssetAtPath<CraftingRecipeSO>(path);
            if (recipe == null)
            {
                recipe = ScriptableObject.CreateInstance<CraftingRecipeSO>();
                recipe.recipeId = id;
                recipe.recipeName = name;
                recipe.description = desc;
                recipe.resultItem = resultItem;
                recipe.resultAmount = resultCount;
                recipe.ingredients = new List<RecipeIngredient>();

                foreach (var ing in ingredients)
                {
                    recipe.ingredients.Add(new RecipeIngredient { item = ing.item, amount = ing.count });
                }

                AssetDatabase.CreateAsset(recipe, path);
            }
            return recipe;
        }

        private static void CreateTreeResourceNode(Vector3 pos, int id, Material trunkMat, Material foliageMat, Material stumpMat, ItemDataSO dropItem, Transform parent)
        {
            var treeObj = new GameObject($"Tree_Node_{id}");
            treeObj.transform.SetParent(parent);
            treeObj.transform.position = pos;

            var col = treeObj.AddComponent<CapsuleCollider>();
            col.height = 3.5f;
            col.radius = 0.5f;
            col.center = new Vector3(0f, 1.75f, 0f);

            var visualRoot = new GameObject("Visual");
            visualRoot.transform.SetParent(treeObj.transform);
            visualRoot.transform.localPosition = Vector3.zero;

            var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "Trunk";
            trunk.transform.SetParent(visualRoot.transform);
            trunk.transform.localPosition = new Vector3(0f, 1.25f, 0f);
            trunk.transform.localScale = new Vector3(0.5f, 1.25f, 0.5f);
            trunk.GetComponent<Renderer>().sharedMaterial = trunkMat;
            Object.DestroyImmediate(trunk.GetComponent<Collider>());

            var foliage = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            foliage.name = "Foliage";
            foliage.transform.SetParent(visualRoot.transform);
            foliage.transform.localPosition = new Vector3(0f, 2.7f, 0f);
            foliage.transform.localScale = new Vector3(2.2f, 1.8f, 2.2f);
            foliage.GetComponent<Renderer>().sharedMaterial = foliageMat;
            Object.DestroyImmediate(foliage.GetComponent<Collider>());

            var stump = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stump.name = "DepletedStump";
            stump.transform.SetParent(treeObj.transform);
            stump.transform.localPosition = new Vector3(0f, 0.15f, 0f);
            stump.transform.localScale = new Vector3(0.55f, 0.15f, 0.55f);
            stump.GetComponent<Renderer>().sharedMaterial = stumpMat;
            Object.DestroyImmediate(stump.GetComponent<Collider>());
            stump.SetActive(false);

            var node = treeObj.AddComponent<ResourceNode>();
            SetPrivateField(node, "resourceType", ResourceType.Wood);
            SetPrivateField(node, "dropItemData", dropItem);
            SetPrivateField(node, "hitsToDeplete", 3);
            SetPrivateField(node, "respawnTime", 20f);
            SetPrivateField(node, "intactVisual", visualRoot.transform);
            SetPrivateField(node, "depletedVisual", stump.transform);
            SetPrivateField(node, "nodeCollider", col);
        }

        private static void CreateRockResourceNode(Vector3 pos, int id, Material stoneMat, ItemDataSO dropItem, Transform parent)
        {
            var rockObj = new GameObject($"Rock_Node_{id}");
            rockObj.transform.SetParent(parent);
            rockObj.transform.position = pos;

            var col = rockObj.AddComponent<SphereCollider>();
            col.radius = 0.9f;
            col.center = new Vector3(0f, 0.7f, 0f);

            var visualRoot = new GameObject("Visual");
            visualRoot.transform.SetParent(rockObj.transform);
            visualRoot.transform.localPosition = Vector3.zero;

            var mainRock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mainRock.name = "Main_Boulder";
            mainRock.transform.SetParent(visualRoot.transform);
            mainRock.transform.localPosition = new Vector3(0f, 0.65f, 0f);
            mainRock.transform.localScale = new Vector3(1.3f, 1.1f, 1.3f);
            mainRock.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(10f, 25f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(10f, 25f));
            mainRock.GetComponent<Renderer>().sharedMaterial = stoneMat;
            Object.DestroyImmediate(mainRock.GetComponent<Collider>());

            var depletedRock = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            depletedRock.name = "DepletedRock";
            depletedRock.transform.SetParent(rockObj.transform);
            depletedRock.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            depletedRock.transform.localScale = new Vector3(1.1f, 0.1f, 1.1f);
            depletedRock.GetComponent<Renderer>().sharedMaterial = stoneMat;
            Object.DestroyImmediate(depletedRock.GetComponent<Collider>());
            depletedRock.SetActive(false);

            var node = rockObj.AddComponent<ResourceNode>();
            SetPrivateField(node, "resourceType", ResourceType.Stone);
            SetPrivateField(node, "dropItemData", dropItem);
            SetPrivateField(node, "hitsToDeplete", 4);
            SetPrivateField(node, "respawnTime", 25f);
            SetPrivateField(node, "intactVisual", visualRoot.transform);
            SetPrivateField(node, "depletedVisual", depletedRock.transform);
            SetPrivateField(node, "nodeCollider", col);
        }

        private static void CreateBerryBushNode(Vector3 pos, int id, Material bushMat, Material berryMat, ItemDataSO dropItem, Transform parent)
        {
            var bushObj = new GameObject($"Berry_Bush_{id}");
            bushObj.transform.SetParent(parent);
            bushObj.transform.position = pos;

            var col = bushObj.AddComponent<SphereCollider>();
            col.radius = 0.7f;
            col.center = new Vector3(0f, 0.5f, 0f);

            var visualRoot = new GameObject("Visual");
            visualRoot.transform.SetParent(bushObj.transform);
            visualRoot.transform.localPosition = Vector3.zero;

            var bush = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bush.name = "BushFoliage";
            bush.transform.SetParent(visualRoot.transform);
            bush.transform.localPosition = new Vector3(0f, 0.45f, 0f);
            bush.transform.localScale = new Vector3(1.1f, 0.8f, 1.1f);
            bush.GetComponent<Renderer>().sharedMaterial = bushMat;
            Object.DestroyImmediate(bush.GetComponent<Collider>());

            Vector3[] berryOffsets = new Vector3[]
            {
                new Vector3(0.35f, 0.6f, 0.2f),
                new Vector3(-0.3f, 0.55f, 0.3f),
                new Vector3(0.1f, 0.7f, -0.35f)
            };
            foreach (var offset in berryOffsets)
            {
                var berry = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                berry.name = "Berry";
                berry.transform.SetParent(visualRoot.transform);
                berry.transform.localPosition = offset;
                berry.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                berry.GetComponent<Renderer>().sharedMaterial = berryMat;
                Object.DestroyImmediate(berry.GetComponent<Collider>());
            }

            var node = bushObj.AddComponent<ResourceNode>();
            SetPrivateField(node, "resourceType", ResourceType.Food);
            SetPrivateField(node, "dropItemData", dropItem);
            SetPrivateField(node, "hitsToDeplete", 1);
            SetPrivateField(node, "respawnTime", 15f);
            SetPrivateField(node, "intactVisual", visualRoot.transform);
            SetPrivateField(node, "nodeCollider", col);
        }

        private static void CreateBoundaryWalls(GameObject parent, Material mat)
        {
            float size = 25f;
            float thickness = 1f;
            float height = 2.5f;

            CreateWall(parent, "Wall_North", new Vector3(0f, height * 0.5f, size), new Vector3(size * 2, height, thickness), mat);
            CreateWall(parent, "Wall_South", new Vector3(0f, height * 0.5f, -size), new Vector3(size * 2, height, thickness), mat);
            CreateWall(parent, "Wall_East", new Vector3(size, height * 0.5f, 0f), new Vector3(thickness, height, size * 2), mat);
            CreateWall(parent, "Wall_West", new Vector3(-size, height * 0.5f, 0f), new Vector3(thickness, height, size * 2), mat);
        }

        private static void CreateWall(GameObject parent, string name, Vector3 pos, Vector3 scale, Material mat)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(parent.transform);
            wall.transform.position = pos;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = mat;
        }

        private static Material GetOrCreateMaterial(string path, Color color, Shader shader)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(shader) { color = color };
                AssetDatabase.CreateAsset(mat, path);
            }
            return mat;
        }

        private static SurvivalConfigSO GetOrCreateSurvivalConfig()
        {
            string path = $"{ConfigFolder}/SurvivalConfig_Default.asset";
            SurvivalConfigSO config = AssetDatabase.LoadAssetAtPath<SurvivalConfigSO>(path);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<SurvivalConfigSO>();
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
            }
            return config;
        }

        private static WeaponDataSO GetOrCreateWeaponData()
        {
            string path = $"{ItemsFolder}/Weapon_IronSword_T1.asset";
            WeaponDataSO weapon = AssetDatabase.LoadAssetAtPath<WeaponDataSO>(path);
            if (weapon == null)
            {
                weapon = ScriptableObject.CreateInstance<WeaponDataSO>();
                weapon.weaponId = "weapon_sword_iron_t1";
                weapon.weaponName = "Trường Kiếm Sắt Ấp Làng";
                weapon.category = WeaponCategory.Sword;
                weapon.damageType = DamageType.Physical;
                weapon.baseDamage = 25f;
                weapon.attackRange = 2.5f;
                weapon.attackAngle = 110f;
                weapon.attackCooldown = 0.45f;
                weapon.knockbackForce = 3.0f;

                AssetDatabase.CreateAsset(weapon, path);
                AssetDatabase.SaveAssets();
            }
            return weapon;
        }

        private static void EnsureDirectories()
        {
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "Data");
            }
            if (!AssetDatabase.IsValidFolder(ItemsFolder))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Items");
            }
            if (!AssetDatabase.IsValidFolder(RecipesFolder))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Recipes");
            }
            if (!AssetDatabase.IsValidFolder(ConfigFolder))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Config");
            }
            if (!AssetDatabase.IsValidFolder(MaterialsFolder))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Art"))
                {
                    AssetDatabase.CreateFolder("Assets/_Project", "Art");
                }
                AssetDatabase.CreateFolder("Assets/_Project/Art", "Materials");
            }
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(target, value);
        }

        private static void AddSceneToBuildSettings(string path)
        {
            var scenes = EditorBuildSettings.scenes;
            foreach (var s in scenes)
            {
                if (s.path == path) return;
            }
            var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            scenes.CopyTo(newScenes, 0);
            newScenes[newScenes.Length - 1] = new EditorBuildSettingsScene(path, true);
            EditorBuildSettings.scenes = newScenes;
        }
    }
}

