using Anubis.LC.ExtraDays.Extensions;
using Anubis.LC.ExtraDays.Helpers;
using BepInEx.Configuration;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Anubis.LC.ExtraDays.ModNetwork
{
    internal class Networking : NetworkBehaviour
    {
        public static Networking Instance;

        public int extraDaysPrice = 0;
        public bool isCorrelatedPrice = false;
        public bool isReduceBuyingRate = false;

        [ServerRpc(RequireOwnership = false)]
        public void SetReduceBuyingRateServerRpc(bool _isReduce)
        {
            if (!IsHost) return;
            ModStaticHelper.Logger.LogInfo($"Setup correlated price for everyone {_isReduce}");
            SetReduceBuyingRateClientRpc(_isReduce);
        }

        [ClientRpc]
        public void SetReduceBuyingRateClientRpc(bool _isReduce)
        {
            ModStaticHelper.Logger.LogInfo($"Setup extra day price for player, {_isReduce}");
            isReduceBuyingRate = _isReduce;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetCorrelatedPriceServerRpc(bool _isCorrelated)
        {
            if (!IsHost) return;
            ModStaticHelper.Logger.LogInfo($"Setup correlated price for everyone {_isCorrelated}");
            SetCorrelatedPriceClientRpc(_isCorrelated);
            if(_isCorrelated)
            {
                SetExtraDaysPriceServerRpc(null);
            }
        }

        [ClientRpc]
        public void SetCorrelatedPriceClientRpc(bool _isCorrelated)
        {
            ModStaticHelper.Logger.LogInfo($"Setup extra day price for player, {_isCorrelated}");
            isCorrelatedPrice = _isCorrelated;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetExtraDaysPriceServerRpc(int? _extraDaysPrice)
        {
            if (!IsHost) return;
            TimeOfDay.Instance.SetExtraDaysPrice();
            var price = _extraDaysPrice.HasValue ? _extraDaysPrice.Value : TimeOfDay.Instance.GetExtraDaysPrice();
            ModStaticHelper.Logger.LogInfo($"Setup extra day price for everyone {price}");
            SetExtraDaysPriceClientRpc(price);
        }

        [ClientRpc]
        public void SetExtraDaysPriceClientRpc(int _extraDaysPrice)
        {
            ModStaticHelper.Logger.LogInfo($"Setup extra day price for player, {_extraDaysPrice}");
            extraDaysPrice = _extraDaysPrice;
            TimeOfDay.Instance.SyncTimeAndDeadline();
        }

        [ServerRpc(RequireOwnership = false)]
        public void BuyExtraDayServerRpc()
        {
            BuyExtraDayClientRpc();
        }

        [ClientRpc]
        public void BuyExtraDayClientRpc()
        {
            if (!IsHost) return;
            Terminal terminal = Object.FindObjectOfType<Terminal>();
            if (terminal == null)
            {
                ModStaticHelper.Logger.LogWarning("Could not find Terminal object. Might be sync issues");
                return;
            }
            terminal.SetDaysToDeadline();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SyncTimeServerRpc()
        {
            SyncTimeClientRpc();
        }

        [ClientRpc]
        public void SyncTimeClientRpc()
        {
            TimeOfDay.Instance.SyncTimeAndDeadline();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                StartCoroutine(WaitForSomeTime());
            }
        }

        private IEnumerator WaitForSomeTime()
        {
            // We need to wait because sending an RPC before a NetworkObject is spawned results in errors.
            yield return new WaitUntil(() => NetworkObject.IsSpawned);
            if (LethalConfigHelper.GetConfigForSaveFile().TryGetValue("correlatedPrice", out var correlatedPrice))
            {
                SetCorrelatedPriceServerRpc(((ConfigEntry<bool>)correlatedPrice).Value);
            }
            if (LethalConfigHelper.GetConfigForSaveFile().TryGetValue("buyingRate", out var _isReduceBuyingRate))
            {
                SetReduceBuyingRateServerRpc(((ConfigEntry<bool>)_isReduceBuyingRate).Value);
            }
            if (LethalConfigHelper.GetConfigForSaveFile().TryGetValue("extraDayPrice", out var _extraDayPrice))
            {
                SetExtraDaysPriceServerRpc(((ConfigEntry<int>)_extraDayPrice).Value);
                SyncTimeServerRpc();
            }
        }
    }
}
