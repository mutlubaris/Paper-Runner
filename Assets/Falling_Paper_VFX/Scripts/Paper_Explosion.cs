using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper_Explosion : MonoBehaviour {

    public GameObject paperFX;
    public GameObject table;
    public GameObject tableBroken;
    public GameObject stackOfPaper;

    private bool exploding = false;

    // Use this for initialization
    void Start () {

        paperFX.SetActive(false);
        tableBroken.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Jump"))
        {

            if (exploding == false)
            {

                StartCoroutine("Explode");

            }

        }

    }

    IEnumerator Explode()
    {

        exploding = true;

        paperFX.SetActive(true);
        table.SetActive(false);
        tableBroken.SetActive(true);
        stackOfPaper.SetActive(false);

        yield return new WaitForSeconds(15.0f);

        paperFX.SetActive(false);

        exploding = false;

    }

}
