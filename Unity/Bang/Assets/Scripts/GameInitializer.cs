using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    public GameObject Tablet;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public GameObject Card;

    private const int LivePoints = 5;

    private List<string> characters;
    private List<string> roles;
    private List<string> playingCards;
    private Transform handArea;

    private List<GameObject> tablets;

    void Start()
    {
        FillLists();
        Initialize();
    }

    // TODO use Domain method to retrive all cards
    private void FillLists()
    {

        characters = new List<string>
        {
            "Images/Characters/pedro_ramirez",
        };
        roles = new List<string>
        {
            "Images/Roles/sheriff"
        };
        playingCards = new List<string>
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

        tablets = new List<GameObject>
        {
            Tablet,
            Enemy1,
            Enemy2,
            Enemy3
        };
    }

    private void Initialize()
    {
        foreach(var tablet in tablets)
        {
            handArea = tablet.transform.Find("HandCardsArea");
            var role = tablet.transform.Find("RoleCard");
            var spriteName = roles[Random.Range(0, roles.Count)];
            role.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteName);

            var characterCard = tablet.transform.Find("CharacterCard");
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
            
    }
}
