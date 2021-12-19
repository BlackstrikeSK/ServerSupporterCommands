using Exiled.API.Extensions;
using Exiled.API.Features;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using Log = Exiled.API.Features.Log;
using MEC;

namespace ServerSupporterCommands
{
    public class EventsHandler
    {

        public IEnumerator<float> HardenedCountdown(Player player, float cooldown)
        {
            float CurrentCooldown = cooldown;

            while(CurrentCooldown > 0f)
            {
                CurrentCooldown--;

                HardenedCooldown.Remove(player);
                HardenedCooldown.Add(player, ((int)CurrentCooldown));

                yield return CurrentCooldown;
                yield return Timing.WaitForSeconds(1f);
            }
            yield return CurrentCooldown;
            yield return Timing.WaitForSeconds(1f);
        }

        public IEnumerator<float> SpeedCountdown(Player player, float cooldown)
        {
            float CurrentCooldown = cooldown;

            while (CurrentCooldown > 0f)
            {
                CurrentCooldown--;

                SpeedCooldown.Remove(player);
                SpeedCooldown.Add(player, ((int)CurrentCooldown));

                yield return CurrentCooldown;
                yield return Timing.WaitForSeconds(1f);
            }
            yield return CurrentCooldown;
            yield return Timing.WaitForSeconds(1f);
        }

        public static void SetPlayerScale(GameObject target, float x, float y, float z)
        {
            try
            {
                NetworkIdentity identity = target.GetComponent<NetworkIdentity>();
                target.transform.localScale = new Vector3(1 * x, 1 * y, 1 * z);

                ObjectDestroyMessage destroyMessage = new ObjectDestroyMessage
                {
                    netId = identity.netId
                };

                foreach (GameObject player in PlayerManager.players)
                {
                    NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;
                    if (player != target)
                        playerCon.Send(destroyMessage, 0);

                    object[] parameters = new object[] { identity, playerCon };
                    typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
                }
            }
            catch (Exception e)
            {
                Log.Info($"Set Scale error: {e}");
            }
        }

        public Dictionary<Player, int> SpeedUses { get; set; } = new Dictionary<Player, int>();
        public Dictionary<Player, int> CoinUses { get; set; } = new Dictionary<Player, int>();
        public Dictionary<Player, int> HardenedUses { get; set; } = new Dictionary<Player, int>();

        public Dictionary<Player, int> HardenedCooldown { get; set; } = new Dictionary<Player, int>();
        public Dictionary<Player, int> SpeedCooldown { get; set; } = new Dictionary<Player, int>();

        public void OnRestartingRound()
        {
            SpeedUses.Clear();
            CoinUses.Clear();
            HardenedUses.Clear();

            SpeedCooldown.Clear();
            HardenedCooldown.Clear();
        }


    }
}