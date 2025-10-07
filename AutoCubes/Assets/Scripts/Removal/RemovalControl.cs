using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RemovalControl : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabBoxes;
    [SerializeField] private GameObject[] prefabs3d;
    [SerializeField] private RemovalUI removalUI;

    private RemovalBox removalBox = null;
    private List<GameObject> orders = new List<GameObject>();
    private int numBox = -1;
    private float maxX = 8.5f, minX = -8.5f;
    private float posX = -8.5f, posZ = 4.5f;
    private float timer = 1f;
    private int countSecond = 0;
    private int totalSecond = 300;

    private int[] ArrBaseExp = { 100, 150, 200, 250};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numBox = GameManager.Instance.currentPlayer.numBoxRemoval;
        totalSecond = GameManager.Instance.currentPlayer.countSecondRemoval;
        GameManager.Instance.currentPlayer.currentScore = 0;
        //numBox = 3;
        GameObject box = Instantiate(prefabBoxes[numBox], new Vector3(0, 0f, 0.7f), Quaternion.identity);
        removalBox = box.GetComponent<RemovalBox>();
        removalBox.SetRemovalControl(gameObject.GetComponent<RemovalControl>());
        removalUI.ViewClock(totalSecond);
        removalUI.ChangeBtnUndoInteractable(false);
        GenerateOrders();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            timer = 1f;
            countSecond++;
            if (totalSecond - countSecond > 0) removalUI.ViewClock(totalSecond - countSecond);
            else
            {   //  время вышло - проиграл ?!
                removalUI.ViewClock(0);
                removalUI.ViewLossPanel(removalBox.CountCurrentCeils, removalBox.CountCeils);
            }
        }
    }

    public void Restart()
    {
        totalSecond = GameManager.Instance.currentPlayer.countSecondRemoval;
        removalUI.ViewClock(totalSecond);
        countSecond = 0;
        removalBox.Restart();
        foreach(GameObject order in orders)
        {
            Order3D order3D = order.GetComponent<Order3D>();
            if (order3D != null) order3D.Restart();
        }
    }

    public void OrderUndo()
    {
        removalBox.Undo();
    }

    private void GenerateOrders()
    {
        int volume = removalBox.CountCeils;
        int countCeils = 0;
        int numOrder = -1, countOrders = 0;
        int startOrder = 2;
        
        orders.Clear();
        while (volume - countCeils > 3)
        {
            numOrder = Random.Range(startOrder, prefabs3d.Length);
            if ((volume - countCeils - prefabs3d[numOrder].GetComponent<Order3D>().CountCeils) < 3) continue;
            GameObject order = CreateOrder(numOrder, countOrders, (numOrder > 3) ? 2.1f : 1.6f);
            countCeils += order.GetComponent<Order3D>().CountCeils;
            //orders.Add(order);
            if (countCeils > 0.4f * volume) startOrder = 1;
            if (countCeils > 0.8f * volume) startOrder = 0;
            countOrders++;
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject order = CreateOrder(0, countOrders, 1.6f);
            countCeils++;
            //orders.Add(order);
            countOrders++;
            if (countCeils == volume) break;
        }
    }

    private GameObject CreateOrder(int numOrder, int countOrders, float dopX = 2.1f)
    {
        Vector3 pos = new Vector3(0, 5.1f, 0);
        if (numBox == 0) pos.y -= 1f;
        GameObject order = Instantiate(prefabs3d[numOrder]);
        Order3D order3D = order.GetComponent<Order3D>();
        float dx = order3D.CX / 2.0f;
        if (posX + dx > maxX) 
        {
            posX = minX;
            posZ -= 2.5f; 
        }
        if (posZ < 3f && posZ > -1f)
        {
            if ((posX + dx > -4f) && (posX + dx < 4f)) posX = 4f;
        }
        pos.x = posX + dx; pos.z = posZ;
        posX += dx + dopX;

        //countCeils += order.GetComponent<Order3D>().CountCeils;
        //pos.x = -7.5f + 2.2f * (countOrders % 8);
        //pos.z = 4.5f - 2.5f * (countOrders / 8);
        order.transform.position = pos;
        orders.Add(order);
        return order;
    }

    public void TranslateOrders(int countOrders, int maxOrders, bool isUndo = true)
    {
        removalUI.ChangeBtnUndoInteractable(isUndo);
        removalUI.ViewOrders(countOrders, maxOrders);
    }

    public void ResetUndoButton()
    {
        removalUI.ChangeBtnUndoInteractable(false);
    }

    public void BoxIsFull(int maxOrders)
    {
        int exp = ArrBaseExp[numBox] + (1 + numBox) * (totalSecond - countSecond);
        GameManager.Instance.currentPlayer.currentScore = exp;
        GameManager.Instance.currentPlayer.sessionGold = ArrBaseExp[numBox];
        removalUI.ViewWinPanel(maxOrders, exp);
    }

    public void AddRewardedExp(int expDop)
    {
        GameManager.Instance.currentPlayer.currentScore += expDop;
        print($"AddRewardedExp expDop={expDop} GM_score={GameManager.Instance.currentPlayer.currentScore}");
        removalUI.ViewExp(GameManager.Instance.currentPlayer.currentScore);
    }
}
