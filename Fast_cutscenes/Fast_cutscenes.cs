using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Globalization;
using System.Resources;
using UnityEngine;
using UnityEngine.Playables;

namespace Fast_cutscenes_lib
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Fast_cutscenes : BaseUnityPlugin
    {
        public const string pluginGuid = "Fast_cutscenes";
        public const string pluginName = "Fast_cutscenes";
        public const string pluginVersion = "1.0.0";

        public const bool logging = false;

        public static ConfigEntry<bool> auto_textskip;
        public static ConfigEntry<string> fast_cutscenes;
        public static ConfigEntry<float> speedtime;

        public static ConfigEntry<bool> skip_bus;
        public static ConfigEntry<bool> skip_pothead;
        public static ConfigEntry<bool> skip_grandma;
        public static ConfigEntry<bool> skip_frog;
        public static ConfigEntry<bool> skip_bard;
        public static ConfigEntry<bool> skip_shop_prompted;
        public static ConfigEntry<bool> skip_lod_clap;

        public static ConfigEntry<bool> fast_chandler;
        public static ConfigEntry<bool> fast_dfs_intro;
        public static ConfigEntry<bool> fast_dfs_kill;
        public static ConfigEntry<bool> fast_gotd_intro;
        public static ConfigEntry<bool> fast_gotd_kill;
        public static ConfigEntry<bool> fast_first_summit;
        public static ConfigEntry<bool> fast_unspecific;
        public static ConfigEntry<bool> fast_boss_kill;
        public static ConfigEntry<bool> fast_boss_soul;
        public static ConfigEntry<bool> fast_gd_after_boss;
        public static ConfigEntry<bool> fast_lod_clap;
        public static ConfigEntry<bool> fast_ff_intro;
        public static ConfigEntry<bool> fast_frog_intro;
        public static ConfigEntry<bool> fast_betty_intro;
        public static ConfigEntry<bool> fast_gd_boss_intro;
        public static ConfigEntry<bool> fast_gd_boss_kill;
        public static ConfigEntry<bool> fast_avarice_entry;
        public static ConfigEntry<bool> fast_avarice_exit;
        public static ConfigEntry<bool> fast_avarice_gift;
        public static ConfigEntry<bool> fast_enter_dd;
        public static ConfigEntry<bool> fast_gc_intro;
        public static ConfigEntry<bool> fast_after_gc;
        public static ConfigEntry<bool> fast_gautlet_intro;
        public static ConfigEntry<bool> fast_lod_intro;
        public static ConfigEntry<bool> fast_lod_kill;
        public static ConfigEntry<bool> fast_credits;
        public static ConfigEntry<bool> fast_oceanspirit;
        public static ConfigEntry<bool> fast_servant_kill;
        public static ConfigEntry<bool> fast_crow_souls;
        public static ConfigEntry<bool> fast_owls;
        public static ConfigEntry<bool> fast_shrines;

        internal static new ManualLogSource Log; //logging (idk if this is needed whatever I have it in other projects too c:)

        public void Awake()
        {
            Log = base.Logger;

            auto_textskip = base.Config.Bind<bool>("General", "auto_textskip", true, new ConfigDescription("Automatically skip all text-boxes."));
            fast_cutscenes = base.Config.Bind(new ConfigDefinition("General", "fast_cutscenes"), "all", new ConfigDescription("Speed up cutscenes.",
                                    new AcceptableValueList<string>("all", "specific", "none")));
            speedtime = base.Config.Bind<float>("General", "speed_time", 20f, new ConfigDescription("Speed value. for cutscenes", new AcceptableValueRange<float>(1f, 20f)));

            skip_bus = base.Config.Bind<bool>("Skipped_cutscenes", "Bus", true, new ConfigDescription("Skip the bus cutscene."));
            skip_pothead = base.Config.Bind<bool>("Skipped_cutscenes", "Pothead Cutscenes", true, new ConfigDescription("Skip all of the Pothead cutscenes."));
            skip_grandma = base.Config.Bind<bool>("Skipped_cutscenes", "Grandma Cutscenes", true, new ConfigDescription("Skip all of the Grandma cutscenes."));
            skip_frog = base.Config.Bind<bool>("Skipped_cutscenes", "Frog Cutscenes", true, new ConfigDescription("Skip all of the Frog cutscenes."));
            skip_bard = base.Config.Bind<bool>("Skipped_cutscenes", "Bard Cutscenes", true, new ConfigDescription("Skip all of the Bard cutscenes."));
            skip_shop_prompted = base.Config.Bind<bool>("Skipped_cutscenes", "Shop cs after DFS", true, new ConfigDescription("Skip the cutscene of the shopkeeper after DFS."));
            skip_lod_clap = base.Config.Bind<bool>("Skipped_cutscenes", "LoD Clap", true, new ConfigDescription("Skip the cutscene of LoD applauding to you."));

            fast_chandler = base.Config.Bind<bool>("Sped_up_cutscenes", "Chandler", true, new ConfigDescription("Speed up chandler cutscene."));
            fast_dfs_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "DFS Intro", true, new ConfigDescription("Speed up DFS intro cutscene."));
            fast_dfs_kill = base.Config.Bind<bool>("Sped_up_cutscenes", "DFS Kill", true, new ConfigDescription("Speed up DFS kill cutscene."));
            fast_gotd_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "GOTD Intro", true, new ConfigDescription("Speed up GOTD intro cutscene."));
            fast_gotd_kill = base.Config.Bind<bool>("Sped_up_cutscenes", "GOTD Kill", true, new ConfigDescription("Speed up GOTD kill cutscene."));
            fast_first_summit = base.Config.Bind<bool>("Sped_up_cutscenes", "1st summit", true, new ConfigDescription("Speed up 1st summit cutscene."));
            fast_unspecific = base.Config.Bind<bool>("Sped_up_cutscenes", "unspecified", true, new ConfigDescription("Speed up cutscenes that are just names CUTSCENE like grandma vault talk for example."));
            fast_boss_kill = base.Config.Bind<bool>("Sped_up_cutscenes", "Boss death scream", true, new ConfigDescription("Speed up boss kill animations."));
            fast_boss_soul = base.Config.Bind<bool>("Sped_up_cutscenes", "Boss soul cutscene", true, new ConfigDescription("Speed up the rotating soul cutscene after boss kill."));
            fast_gd_after_boss = base.Config.Bind<bool>("Sped_up_cutscenes", "GD cs after Boss kill", true, new ConfigDescription("Speed up the cutscene of gravedigger burring Bosses."));
            fast_lod_clap = base.Config.Bind<bool>("Sped_up_cutscenes", "LoD Clap", true, new ConfigDescription("Speed up the cutscene of LoD applauding to you."));
            fast_ff_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "FF Intro", true, new ConfigDescription("Speed up Flooded Fortress intro cutscene."));
            fast_frog_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "Frog Intro", true, new ConfigDescription("Speed up Frog intro cutscene."));
            fast_betty_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "Betty Intro", true, new ConfigDescription("Speed up Betty intro cutscene."));
            fast_gd_boss_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "Gravedigger Boss Intro", true, new ConfigDescription("Speed up gravedigger boss intro cutscene."));
            fast_gd_boss_kill = base.Config.Bind<bool>("Sped_up_cutscenes", "Gravedigger Boss Kill", true, new ConfigDescription("Speed up gravedigger boss death cutscene."));
            fast_avarice_entry = base.Config.Bind<bool>("Sped_up_cutscenes", "Avarice Entry", true, new ConfigDescription("Speed up avarice entry cutscene."));
            fast_avarice_exit = base.Config.Bind<bool>("Sped_up_cutscenes", "Avarice Exit", true, new ConfigDescription("Speed up avarice exit cutscene."));
            fast_avarice_gift = base.Config.Bind<bool>("Sped_up_cutscenes", "Avarice Gift", true, new ConfigDescription("Speed up avarice recieving gift cutscene."));
            fast_enter_dd = base.Config.Bind<bool>("Sped_up_cutscenes", "Enter Deaths Door", true, new ConfigDescription("Speed up entering deaths door cutscene."));
            fast_gc_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "Grey crow Intro", true, new ConfigDescription("Speed up grex crow intro cutscene."));
            fast_after_gc = base.Config.Bind<bool>("Sped_up_cutscenes", "Death chat after gc", true, new ConfigDescription("Speed up the cutscene of chatting with death after killing grey crow."));
            fast_gautlet_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "LoD Gauntlet Intro", true, new ConfigDescription("Speed up the intro cutscene of the first gauntlet fight."));
            fast_lod_intro = base.Config.Bind<bool>("Sped_up_cutscenes", "LoD Bossfight Intro", true, new ConfigDescription("Speed up the final LoD bossfight intro cutscene."));
            fast_lod_kill = base.Config.Bind<bool>("Sped_up_cutscenes", "LoD Bossfight Kill", true, new ConfigDescription("Speed up the final LoD bossfight kill cutscene."));
            fast_credits = base.Config.Bind<bool>("Sped_up_cutscenes", "Credits", true, new ConfigDescription("Speed up the credits cutscene."));
            fast_oceanspirit = base.Config.Bind<bool>("Sped_up_cutscenes", "Ocean Spirit", true, new ConfigDescription("Speed up cutscene bringing Jefferson to the ocean spirit."));
            fast_servant_kill = base.Config.Bind<bool>("Sped_up_cutscenes", "Silent Servant Kill", true, new ConfigDescription("Speed up silent servant kill cutscene."));
            fast_crow_souls = base.Config.Bind<bool>("Sped_up_cutscenes", "Crow Souls", true, new ConfigDescription("Speed up the cutscenes of crow souls flying to ancient doors."));
            fast_owls = base.Config.Bind<bool>("Sped_up_cutscenes", "Owls", true, new ConfigDescription("Speed up the owl cutscenes."));
            fast_shrines = base.Config.Bind<bool>("Sped_up_cutscenes", "Shrines", true, new ConfigDescription("Speed up the shrine cutscenes."));

            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll(typeof(Fast_cutscenes));
        }

        private void Update()
        {
            if (GameTimeTracker.instance != null)
            {
                if (auto_textskip.Value)
                {
                    Skip_Text();
                }
            }
        }

        private void Skip_Text()
        {
            foreach (NPCCharacter i in Resources.FindObjectsOfTypeAll<NPCCharacter>())
            {
                if (!i.IsFinished())
                {
                    i.NextLine();
                }
            }
        }


        [HarmonyPatch(typeof(GameSave), "SetKeyState")]
        [HarmonyPostfix]
        public static void SetKeyState_new(string id)
        {
            if (id == "cts_handler" & skip_lod_clap.Value)
            {
                GameSave.GetSaveData().SetKeyState("c_met_lod", true, true);
            }
            if (logging) { Log.LogWarning("Set Key: " + id); }
        }

        [HarmonyPatch(typeof(GrandmaBoss), "FixedUpdate")]
        [HarmonyPostfix]
        public static void Fast_cs_on_gm(GrandmaBoss __instance)
        {
            if ((fast_cutscenes.Value == "all" || fast_boss_kill.Value) & Time.timeScale < speedtime.Value & (fast_cutscenes.Value != "none"))
            {
                AI_Brain.AIState state = __instance.GetState();
                if (state == AI_Brain.AIState.Dead)
                {
                    Fast_cs_on();
                }
            }
        }

        [HarmonyPatch(typeof(AI_SilentServant), "FixedUpdate")]
        [HarmonyPostfix]
        public static void Fast_cs_on_Servant(AI_SilentServant __instance)
        {
            if ((fast_cutscenes.Value == "all" || fast_servant_kill.Value) & Time.timeScale < speedtime.Value & (fast_cutscenes.Value != "none"))
            {
                AI_Brain.AIState state = __instance.GetState();
                if (state == AI_Brain.AIState.Dead)
                {
                    Fast_cs_on();
                }
            }
        }

        [HarmonyPatch(typeof(RedeemerCorpse), "Start")]
        [HarmonyPostfix]
        public static void Fast_cs_on_gotd(RedeemerCorpse __instance, BaseKey ___completeKey)
        {
            if (((fast_cutscenes.Value == "all") || fast_gotd_kill.Value) & (fast_cutscenes.Value != "none"))
            {
                UnityEngine.Object.Destroy(__instance.gameObject);
                ___completeKey.Unlock();
            }
        }

        [HarmonyPatch(typeof(SoulEmerge), "updateSoulGlow")]
        [HarmonyPostfix]
        public static void Fast_cs_on_boss_kill()
        {
            if ((fast_cutscenes.Value == "all" || fast_boss_kill.Value) & Time.timeScale < speedtime.Value & (fast_cutscenes.Value != "none"))
            {
                Fast_cs_on();
            }
        }

        [HarmonyPatch(typeof(SoulAbsorbCutscene), "Update")]
        [HarmonyPostfix]
        public static void Fast_cs_on_Soul()
        {
            if ((fast_cutscenes.Value == "all" || fast_boss_soul.Value) & Time.timeScale < speedtime.Value & (fast_cutscenes.Value != "none"))
            {
                Fast_cs_on();
            }
        }

        [HarmonyPatch(typeof(SoulKey), "Trigger")]
        [HarmonyPostfix]
        public static void Fast_cs_on_CrowSoul()
        {
            if ((fast_cutscenes.Value == "all" || fast_crow_souls.Value) & Time.timeScale < speedtime.Value & (fast_cutscenes.Value != "none"))
            {
                Fast_cs_on();
            }
        }

        [HarmonyPatch(typeof(Owl), "Trigger")]
        [HarmonyPostfix]
        public static void Fast_cs_on_Owl()
        {
            if ((fast_cutscenes.Value == "all" || fast_owls.Value) & Time.timeScale < speedtime.Value & (fast_cutscenes.Value != "none"))
            {
                Fast_cs_on();
            }
        }

        [HarmonyPatch(typeof(PlayerGlobal), "UnPauseInput_Cutscene")]
        [HarmonyPostfix]
        public static void Fast_cs_off_unpause_cut()
        {
            if (!PlayerGlobal.instance.IsInputPausedCutscene() & Time.timeScale == speedtime.Value)
            {
                Fast_cs_off();
            }
        }

        [HarmonyPatch(typeof(PlayerGlobal), "UnPauseInput_Talk")]
        [HarmonyPostfix]
        public static void Fast_cs_off_unpause_talk()
        {
            if (!PlayerGlobal.instance.IsInputPausedCutscene() & Time.timeScale == speedtime.Value)
            {
                Fast_cs_off();
            }
        }

        [HarmonyPatch(typeof(Cutscene), "FixedUpdate")]
        [HarmonyPostfix]
        public static void Cutscene_new(Cutscene __instance, bool ___playing, PlayableDirector ___timeline)
        {
            if ((___playing & Time.timeScale < speedtime.Value & ((___timeline.duration - ___timeline.time) > 0.2) & (fast_cutscenes.Value != "none")) &
                ((__instance.name == "Cutscene_Handler" & fast_chandler.Value) ||
                (__instance.name == "_FORESTMOTHER_INTRO" & fast_dfs_intro.Value) ||
                (__instance.name == "CUTSCENE_Steal" & fast_dfs_kill.Value) ||
                (__instance.name == "CUTSCENE_RedeemerIntro" & fast_gotd_intro.Value) ||
                (__instance.name == "CUTSCENE_CrowIntro" & fast_first_summit.Value) ||
                (__instance.name == "Cutscene" & fast_unspecific.Value) ||
                ((__instance.name == "GD_CUTSCENE" || __instance.name == "GD_CUTSCENE_2") & fast_gd_after_boss.Value) ||
                (__instance.name == "Cutscene_MEET" & fast_lod_clap.Value) ||
                (__instance.name == "CUTSCENE_FrogIntro" & fast_ff_intro.Value) ||
                (__instance.name == "CUTSCENE_FrogBOSSIntro" & fast_frog_intro.Value) ||
                (__instance.name == "BETTY_INTRO_CUTSCENE" & fast_betty_intro.Value) ||
                (__instance.name == "GD_FightIntro" & fast_gd_boss_intro.Value) ||
                (__instance.name == "GD_Death" & fast_gd_boss_kill.Value) ||
                ((__instance.name == "Cutscene_Enter_Fire" || __instance.name == "Cutscene_Enter_Hookshot" || __instance.name == "Cutscene_Enter_Bomb" || __instance.name == "Cutscene_Enter") & fast_avarice_entry.Value) ||
                (__instance.name == "Cutscene_Exit" & fast_avarice_exit.Value) ||
                (__instance.name == "Cutscene_Gift" & fast_avarice_gift.Value) ||
                (__instance.name == "6: OPENING" & fast_enter_dd.Value) ||
                (__instance.name == "Cutscene_CrowIntro" & fast_gc_intro.Value) ||
                (__instance.name == "8: OPEN CROW DEAD" & fast_after_gc.Value) ||
                (__instance.name == "CUTSCENE_Intro" & fast_gautlet_intro.Value) ||
                (__instance.name == "CUTSCENE_LodIntro" & fast_lod_intro.Value) ||
                (__instance.name == "CUTSCENE_LodDeath" & fast_lod_kill.Value) ||
                (__instance.name == "CUTSCENE_LodEulogy" & fast_credits.Value) ||
                (__instance.name == "CUTSCENE_OceanSpirit" & fast_oceanspirit.Value) ||
                ((__instance.name == "SHRINE_Crow_Arrows Variant" || __instance.name == "SHRINE_Crow") & fast_shrines.Value)))
            {
                Fast_cs_on("Cutscene: " + __instance.name);
            }
            else if (___playing & Time.timeScale == speedtime.Value & ((___timeline.duration - ___timeline.time) < 0.2))
            {
                Fast_cs_off("Cutscene: " + __instance.name);
            }
        }

        private static void Fast_cs_on(string name = null)
        {
            if (name == null)
            {
                name = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name;
            }
            if (logging) { Log.LogWarning("Fast: " + name); }
            Time.timeScale = speedtime.Value;
        }

        private static void Fast_cs_off(string name = null)
        {
            if (Time.timeScale == speedtime.Value)
            {
                if (name == null)
                {
                    name = "Method: " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name;
                }
                if (logging) { Log.LogWarning("Slow: " + name); }
                Time.timeScale = 1f;
            }
        }

        [HarmonyPatch(typeof(SaveSlot), "useSaveFile")]
        [HarmonyPostfix]
        public static void LoadDefaultValues()
        {
            if (!GameSave.GetSaveData().IsKeyUnlocked("cts_bus"))
            {
                if (skip_bus.Value)
                {
                    GameSave.GetSaveData().SetKeyState("cts_bus", true, true);
                }
                if (skip_pothead.Value)
                {
                    GameSave.GetSaveData().SetKeyState("pothead_intro_1", true, true);
                    GameSave.GetSaveData().SetKeyState("pothead_intro_2", true, true);
                    GameSave.GetSaveData().SetKeyState("pothead_intro_3", true, true);
                    GameSave.GetSaveData().SetKeyState("pothead_confession1", true, true);
                    GameSave.GetSaveData().SetKeyState("pothead_m_4", true, true);
                    GameSave.GetSaveData().SetKeyState("phcs_1", true, true);
                    GameSave.GetSaveData().SetKeyState("phcs_1.5", true, true);
                    GameSave.GetSaveData().SetKeyState("phcs_5", true, true);
                    GameSave.GetSaveData().SetKeyState("phcs_break", true, true);
                    GameSave.GetSaveData().SetKeyState("phcs_2", true, true);
                    GameSave.GetSaveData().SetKeyState("phcs_3", true, true);
                    GameSave.GetSaveData().SetKeyState("ach_pothead", true, true);
                }
                if (skip_grandma.Value)
                {
                    GameSave.GetSaveData().SetKeyState("gm_act_0", true, true);
                    GameSave.GetSaveData().SetKeyState("gm_act_1", true, true);
                    GameSave.GetSaveData().SetKeyState("gm_act_2", true, true);
                    GameSave.GetSaveData().SetKeyState("gm_act_3", true, true);
                    GameSave.GetSaveData().SetKeyState("gran_pot_1", true, true);
                    GameSave.GetSaveData().SetKeyState("gran_pot_2", true, true);
                    GameSave.GetSaveData().SetKeyState("grandma_romp_intro_watched", true, true);
                }
                if (skip_frog.Value)
                {
                    GameSave.GetSaveData().SetKeyState("frog_boss_wall_chat", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_dung_meet_1", true, true);
                    GameSave.GetSaveData().SetKeyState("watched_frogwall", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_boss_swim_chat", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_dung_meet_3", true, true);
                    GameSave.GetSaveData().SetKeyState("watched_frogswim", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_boss_sewer_chat", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_dung_meet_2", true, true);
                    GameSave.GetSaveData().SetKeyState("watched_frogsewer", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_wall_chat_last", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_dung_meet_last", true, true);
                    GameSave.GetSaveData().SetKeyState("frog_ghoul_intro", true, true);
                    GameSave.GetSaveData().SetKeyState("c_swamp_intro", true, true);
                }
                if (skip_bard.Value)
                {
                    GameSave.GetSaveData().SetKeyState("bard_bar_intro", true, true);
                    GameSave.GetSaveData().SetKeyState("bard_cracked_block", true, true);
                    GameSave.GetSaveData().SetKeyState("bard_fort_intro", true, true);
                    GameSave.GetSaveData().SetKeyState("bard_fortress", true, true);
                    GameSave.GetSaveData().SetKeyState("bard_crows", true, true);
                    GameSave.GetSaveData().SetKeyState("bard_betty_cave", true, true);
                    GameSave.GetSaveData().SetKeyState("bard_pre_betty", true, true);
                }

                if (skip_shop_prompted.Value)
                {
                    GameSave.GetSaveData().SetKeyState("shop_prompted", true, true);
                }

                GameSave.SaveGameState();
            }
        }
    }
}
