
using UnityEngine;

public class Alter : MonoBehaviour
{
    public GameObject fireEffect;
    public GameObject portalPosition;
    public GameObject portal;
    
    private void OnCollisionEnter(Collision collision)
    {
        //촛불을 소지했을 때 조건 추가 필요
        if (collision.gameObject.CompareTag("Player"))
        {
            fireEffect.SetActive(true);
            GameObject newPortal = Instantiate(portal, portalPosition.transform.position, portalPosition.transform.rotation);
            newPortal.transform.SetParent(portalPosition.transform);
        }
    }

    
    //테스트용, 나중에 삭제
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            fireEffect.SetActive(true);
            GameObject newPortal = Instantiate(portal, portalPosition.transform.position, portalPosition.transform.rotation);
            newPortal.transform.SetParent(portalPosition.transform);
        }
    }
}
