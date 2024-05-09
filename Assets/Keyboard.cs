using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// public class Keyboard : MonoBehaviour
public class Keyboard : MonoBehaviour
{
    public GameObject blackTile, whiteTile; 
    public GameObject content;
    private int startNote = 24;
    private int[] blackKeyIndex = { 1, 3, 6, 8, 10 };
    private int[] whiteKeyIndex= { 0, 2, 4, 5, 7, 9, 11 };

    public int numberOfOctaves;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfOctaves; i++)createOctave(i);
    }

    private void createOctave(int octave){
        float width = content.GetComponent<RectTransform>().rect.width;
        float widthPerOctave = width / numberOfOctaves; 
        float widthPerNote = widthPerOctave / 7;

        int myStartNote = (octave * 12) + startNote;

        // 7 white tiles
        for (int i = 0; i < 7; i++)
        {
            GameObject note = instantiateNote(whiteTile, whiteKeyIndex[i], myStartNote);
            registerEvents(note);
           
            RectTransform rT = note.GetComponent<RectTransform>();
            RectTransform wT = whiteTile.GetComponent<RectTransform>();

            rT.sizeDelta = new Vector2(widthPerNote - 2, rT.sizeDelta.y);
            rT.anchoredPosition3D = new Vector3(widthPerOctave * octave + widthPerNote * i + widthPerNote / 2, - wT.rect.height / 2, 0);
        } 

        // 5 black tiles 
        for (int i = 0; i < 5; i++)
        {
            GameObject note = instantiateNote(blackTile, blackKeyIndex[i], myStartNote); 
            registerEvents(note);

            RectTransform rT = note.GetComponent<RectTransform>();
            RectTransform bT = blackTile.GetComponent<RectTransform>();

            rT.sizeDelta = new Vector2(widthPerNote/2, rT.sizeDelta.y);

            int blackIndex = i; 
            if(i > 1){ blackIndex += 1; }
            rT.anchoredPosition3D = new Vector3(widthPerOctave*octave+widthPerNote*blackIndex+widthPerNote, - bT.rect.height/2, 0);
        }
    }
    private void registerEvents(GameObject note){
        EventTrigger trigger = note.gameObject.AddComponent<EventTrigger>(); 
        var pointerDown = new EventTrigger.Entry(); 
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => keyOn(note.GetComponent<PianoTile>().midiNote));
        trigger.triggers.Add(pointerDown); 

        var pointerUp = new EventTrigger.Entry(); 
        pointerUp.eventID = EventTriggerType.PointerUp; 
        pointerUp.callback.AddListener((e) => keyOff(note.GetComponent<PianoTile>().midiNote)); 
        trigger.triggers.Add(pointerUp); 
    }

    public void keyOn(int midiNumber){
        Debug.Log("Clicked " + midiNumber); 
        GameObject.Find("SoundGen").GetComponent<SoundGen>().OnKey(midiNumber);
    }

    public void keyOff(int midiNumber){
        Debug.Log("Released " + midiNumber);
        GameObject.Find("SoundGen").GetComponent<SoundGen>().onKeyOff(midiNumber);
    }

    private GameObject instantiateNote(GameObject note, int actualNoteIndex, int startNote){
        GameObject newNote = Instantiate(note);
        newNote.transform.SetParent(content.transform, false);
        newNote.GetComponent<PianoTile>().midiNote = startNote + actualNoteIndex;
        return newNote;
    }
}
