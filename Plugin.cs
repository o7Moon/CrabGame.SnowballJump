using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using Il2CppSystem;

namespace snowballJump
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        // equipping the snowball (slot 4, index 3) should only be allowed when done automatically by code
        public static bool canEquipSnow = false;
        public static int swap_slot_timer = 0;
        // the slot index to swap back to after throwing the snowball
        public static int target_slot = 0;
        public override void Load()
        {
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Harmony.CreateAndPatchAll(typeof(Plugin));
        }
        //                   Snowball
        [HarmonyPatch(typeof(MonoBehaviour1PublicTrtrGahiRiCoBoItVeBoUnique),"OnCollisionEnter")]
        [HarmonyPostfix]
        public static void snowball_hit_patch(MonoBehaviour1PublicTrtrGahiRiCoBoItVeBoUnique __instance, Collision param_1){
            Collider[] exploded = UnityEngine.Physics.OverlapSphere(param_1.contacts[0].point,5f);
            foreach (Collider c in exploded){
                // dont apply any force to snowballs
                if (c.GetComponent<MonoBehaviour1PublicTrtrGahiRiCoBoItVeBoUnique>() != null) continue;
                Rigidbody r = c.GetComponent<Rigidbody>();
                if (r != null) r.AddExplosionForce(700f,param_1.contacts[0].point,10f,0f,ForceMode.Impulse);
            }
        }
        //                   PlayerInventory
        [HarmonyPatch(typeof(MonoBehaviourPublicItDi2ObIninInTrweGaUnique),"EquipItem")]
        [HarmonyPrefix]
        public static bool equipPre(MonoBehaviourPublicItDi2ObIninInTrweGaUnique __instance, int param_1){
            if (param_1 != 3) {
                return true;
            }
            if (!canEquipSnow){
                // skips the equip call
                return false;
            }
            return true;
        }
        //                   PlayerInventory
        [HarmonyPatch(typeof(MonoBehaviourPublicItDi2ObIninInTrweGaUnique),"EquipItem")]
        [HarmonyPostfix]
        public static void equipPost(MonoBehaviourPublicItDi2ObIninInTrweGaUnique __instance){
            canEquipSnow = false;
        }
        [HarmonyPatch(typeof(PlayerInput),"Update")]
        [HarmonyPrefix]
        public static void updateHook(PlayerInput __instance){
            //                             InputManager
            if (Input.GetKeyDown((KeyCode) MonoBehaviourPublicInfobaInlerijuIncrspUnique.rightClick)){
                canEquipSnow = true;
                target_slot = __instance.playerInventory.field_Private_Int32_0;
                __instance.playerInventory.EquipItem(3);
                MonoBehaviour2PublicGathObauTrgumuGaSiBoUnique snowball = __instance.playerInventory.currentItem.GetComponent<MonoBehaviour2PublicGathObauTrgumuGaSiBoUnique>();
                if (snowball != null){
                    snowball.GetReady();
                    __instance.playerInventory.UseItem();
                }
                // the below issue seems to be ping based, so get the ping and use that for the delay
                int ping = (MonoBehaviourPublicStLi1InInUnique.GetPing())/15 + 3;
                // swap back after some amount of FixedUpdate frames, to fix an issue where the snowball doesnt get thrown client-side but does in multiplayer.
                swap_slot_timer = ping;
            }
        }
        [HarmonyPatch(typeof(PlayerInput),"FixedUpdate")]
        [HarmonyPrefix]
        public static void fixedUpdateInput(PlayerInput __instance){
            swap_slot_timer -= 1;
            if (swap_slot_timer == 1){
                __instance.playerInventory.EquipItem(target_slot);
                swap_slot_timer = 0;
            }
        }
    }
}
