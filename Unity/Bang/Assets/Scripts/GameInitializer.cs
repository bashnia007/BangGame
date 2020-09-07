using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    public GameObject Tablet;
    public GameObject Card;

    private const int LivePoints = 5;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var handArea = Tablet.transform.Find("HandCardsArea");

        var characters = new List<string>
        {
            "Images/Characters/pedro_ramirez",
        };
        var roles = new List<string>
        {
            "Images/Roles/sheriff"
        };
        var playingCards = new List<string>
        {
            "Images/PlayingCards/Bang_1",
            "Images/PlayingCards/Bang_2",
            "Images/PlayingCards/Bang_3",
            "Images/PlayingCards/barrel",
            "Images/PlayingCards/Dynamit",
            "Images/PlayingCards/Gatling",
            "Images/PlayingCards/Mustang",
            "Images/PlayingCards/Remington",
            "Images/PlayingCards/Saloon",
            "Images/PlayingCards/Scope",
        };

        var role = Tablet.transform.Find("RoleCard");
        var spriteName = roles[Random.Range(0, roles.Count)];
        role.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteName);

        var characterCard = Tablet.transform.Find("CharacterCard");
        spriteName = characters[Random.Range(0, characters.Count)];
        characterCard.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteName);

        for (int i = 0; i < LivePoints; i++)
        {
            GameObject handCard = Instantiate(Card, new Vector3(0, 0, 0), Quaternion.identity);
            handCard.transform.SetParent(handArea.transform, false);
            spriteName = playingCards[Random.Range(0, playingCards.Count)];
            handCard.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteName);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
