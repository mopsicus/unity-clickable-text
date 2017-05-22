using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TestClick : MonoBehaviour, IPointerClickHandler {

	[SerializeField]
	private TextMeshProUGUI textMessage;

	void Start () {
		CheckLinks ();
	}

	// Get link and open page
	public void OnPointerClick (PointerEventData eventData) {
		int linkIndex = TMP_TextUtilities.FindIntersectingLink (textMessage, eventData.position, eventData.pressEventCamera);
		if (linkIndex == -1) 
			return;
		TMP_LinkInfo linkInfo = textMessage.textInfo.linkInfo[linkIndex];
		string selectedLink = linkInfo.GetLinkID();
		if (selectedLink != "") {
			Debug.LogFormat ("Open link {0}", selectedLink);
			Application.OpenURL (selectedLink);        
		}
	}	    

	// Cut lonk url
	string ShortLink (string link) {
		string text = link;
		int left = 9; 		
		int right = 16; 		
		string cut = "..."; 	
		if (link.Length > (left + right + cut.Length)) 
			text = string.Format ("{0}{1}{2}", link.Substring (0, left), cut, link.Substring (link.Length - right, right));
		return string.Format("<#7f7fe5><u><link=\"{0}\">{1}</link></u></color>", link, text);
	}        

	// Check links in text
	void CheckLinks () {
		Regex regx = new Regex ("((http://|https://|www\\.)([A-Z0-9.-:]{1,})\\.[0-9A-Z?;~&#=\\-_\\./]{2,})" , RegexOptions.IgnoreCase | RegexOptions.Singleline); 
		MatchCollection matches = regx.Matches (textMessage.text); 
		foreach (Match match in matches) 
			textMessage.text = textMessage.text.Replace (match.Value, ShortLink(match.Value));     	
	}

}
