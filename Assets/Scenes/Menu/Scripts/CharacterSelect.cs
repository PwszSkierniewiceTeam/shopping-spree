using Core;
using UnityEngine;

namespace Scenes.Menu.Scripts
{
    public class CharacterSelect : MonoBehaviour
    {
        private bool changed;
        private int charactersCount = 0;
        private int currentCharacter = 0;
        private GameObject[] charactersList;

        // Start is called before the first frame update
        void Start()
        {
            Transform characters = transform.GetChild(0);

            charactersCount = characters.childCount;
            charactersList = new GameObject[characters.childCount];

            for (int i = 0; i < characters.childCount; i++)
            {
                charactersList[i] = characters.GetChild(i).gameObject;
            }

            foreach (GameObject gameObject in charactersList)
            {
                gameObject.SetActive(false);
            }

            if (charactersList[0])
            {
                charactersList[0].SetActive(true);
            }
        }

        private void ChangeCharacter(bool next)
        {
            charactersList[currentCharacter].SetActive(false);

            if (next)
            {
                currentCharacter = currentCharacter < charactersCount - 1 ? currentCharacter + 1 : 0;
            }
            else
            {
                currentCharacter = currentCharacter > 0 ? currentCharacter - 1 : charactersCount - 1;
            }

            charactersList[currentCharacter].SetActive(true);
            
            changed = true;
        }

        // Update is called once per frame
        void Update()
        {
            float axis = Gamepads.Instance.g1.GetJoystickAxis(GamepadJoystick.LeftJoystickHorizontal);

            if (axis == 0)
            {
                changed = false;
            }
            else if (!changed && axis > 0.5f)
            {
                Debug.Log(axis);

                ChangeCharacter(true);
            }
            else if (!changed && axis < -0.5f)
            {
                Debug.Log(axis);
                ChangeCharacter(false);
            }
        }
    }
}