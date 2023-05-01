using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TimerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private int _remainingTime;
    private YieldInstruction _second = new WaitForSeconds(1f);
    private Coroutine _timerRoutine ;
    
    private void OnValidate()
    {
        if (_text == null)
            _text = GetComponent<TMP_Text>();
    }

    public void Init(int time)
    {
        _remainingTime = time;
        SetTime(_remainingTime);
        
        if(_timerRoutine != null)
            StopCoroutine(_timerRoutine);
        
        _timerRoutine = StartCoroutine(TimerRoutine());
    }

    private void SetTime(int time) => _text.text = TimeSpan.FromSeconds(time).ToString()[3..];

    private IEnumerator TimerRoutine()
    {
        while (_remainingTime > 0)
        {
            yield return _second;
            _remainingTime--;
            SetTime(_remainingTime);   
        }
    }
}
