using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] private Button _refreshButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Text _priceText;
    [SerializeField] private Text _statusText;

    private OfferData _currentOffer;

    private Dictionary<string, OfferData> _offerCache = new Dictionary<string, OfferData>();
    private IEnumerable<KeyValuePair<string, OfferData>> _offerCacheView;
    private List<string> _purchasedOffers = new List<string>();

    private void Start()
    {
        _refreshButton.onClick.AddListener(OnRefreshClicked);
        _buyButton.onClick.AddListener(OnBuyClicked);

        _offerCacheView = _offerCache;

        LoadOffer();
    }

    private async void LoadOffer()
    {
        _statusText.text = "Request offer...";

        GetOfferResponse response = await FakeServer.Send(new GetOfferRequest());

        _currentOffer = response.Offer;

        _offerCache.Add(_currentOffer.Id, _currentOffer);

        var best = GetBestOfferFromCache();

        _priceText.text = best != null
            ? best.Price.ToString()
            : _currentOffer.Price.ToString();

        _statusText.text = "Offer loaded";
    }

    private OfferData GetBestOfferFromCache()
    {
        OfferData best = null;

        foreach (var pair in _offerCacheView)
        {
            if (best == null || pair.Value.Price < best.Price)
            {
                best = pair.Value;
            }
        }

        return best;
    }

    private void OnRefreshClicked()
    {
        LoadOffer();
    }

    private async void OnBuyClicked()
    {
        _statusText.text = "Buying...";

        BuyOfferResponse response =
            await FakeServer.Send(new BuyOfferRequest(_currentOffer.Id));

        if (response.Success)
        {
            _statusText.text = "Purchase success";

            _purchasedOffers.Add(_currentOffer.Id);

            LoadOffer();
        }
        else
        {
            _statusText.text = response.Error;
        }
    }
}

#region Server

public static class FakeServer
{
    private static int _coins = 100;

    public static async UniTask<GetOfferResponse> Send(GetOfferRequest request)
    {
        await UniTask.Delay(UnityEngine.Random.Range(500, 2500));

        return new GetOfferResponse
        {
            Offer = new OfferData
            {
                Id = "starter_pack",
                Price = UnityEngine.Random.Range(30, 80),
                EndTime = DateTime.UtcNow.AddMinutes(10)
            },
            Coins = _coins
        };
    }

    public static async UniTask<BuyOfferResponse> Send(BuyOfferRequest request)
    {
        await UniTask.Delay(UnityEngine.Random.Range(1000, 3000));

        int price = UnityEngine.Random.Range(30, 80);

        if (_coins < price)
        {
            return new BuyOfferResponse
            {
                Success = false,
                Error = "Not enough coins"
            };
        }

        _coins -= price;

        return new BuyOfferResponse
        {
            Success = true,
            CoinsLeft = _coins
        };
    }
}

#endregion

#region Requests

public class GetOfferRequest { }

public class BuyOfferRequest
{
    public string OfferId;

    public BuyOfferRequest(string offerId)
    {
        OfferId = offerId;
    }
}

#endregion

#region Responses

public class GetOfferResponse
{
    public OfferData Offer;
    public int Coins;
}

public class BuyOfferResponse
{
    public bool Success;
    public string Error;
    public int CoinsLeft;
}

#endregion

#region Models

public class OfferData
{
    public string Id;
    public int Price;
    public DateTime EndTime;
}

#endregion