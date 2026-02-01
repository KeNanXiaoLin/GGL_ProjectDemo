/*----------------------------------
 *Title:UI自动化组件生成代码生成工具
 *Date:2026/1/31 21:35:51
 *Description:变量需要以[Text]括号加组件类型的格式进行声明,然后右键窗口物体——一键生成UI数据组件脚本即可
 *注意:自动生成的内容仅追加/移除，不会覆盖手写逻辑；新增/删除组件后再次生成，会标记对应代码行
----------------------------------*/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class GrayPanel:MonoBehaviour
{
	private CanvasGroup canvasGroup;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		Hide();
	}

	public void Show()
	{
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
	public void Hide()
	{
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}
}
