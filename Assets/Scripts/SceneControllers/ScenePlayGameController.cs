using UnityEngine;

public class ScenePlayGameController : MonoBehaviour
{
    [SerializeField]
    private Sprite JohnSprite;

    [SerializeField]
    private Sprite AriahSprite;

    [SerializeField]
    private Sprite SteveSprite;


    [SerializeField]
    private GameObject a;


    private void Awake()
    {
        SpawnCharacterBySelectionNumber();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void SpawnCharacterBySelectionNumber()
    {

        GameObject playablePrefab = Resources.Load<GameObject>("Prefabs/PlayableCharacter/Player");
        GameObject playableCharacter = Instantiate(playablePrefab, new Vector3(0, 0, 1), Quaternion.identity);

        SpriteRenderer playerSpriteRenderer = playableCharacter.GetComponent<SpriteRenderer>();

        switch (DataPreserve.characterSelectedNumber)
        {
            case 1: playerSpriteRenderer.sprite = JohnSprite; break;
            case 2: playerSpriteRenderer.sprite = AriahSprite; break;
            case 3: playerSpriteRenderer.sprite = SteveSprite; break;
        }
    }


}
