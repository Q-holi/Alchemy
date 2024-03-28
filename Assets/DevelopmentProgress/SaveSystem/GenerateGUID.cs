using UnityEngine;

[ExecuteAlways]//-- 에디터를 실행시키지 않아도 스크립트 컴포넌트의 콜백 함수가 실행된다
public class GenerateGUID : MonoBehaviour
{
    [SerializeField]
    private string _gUID = "";
    public string GUID { get => _gUID; set => _gUID = value; }

    private void Awake()
    {
        if (!Application.IsPlaying(gameObject))
        {
            if (_gUID == "")
                _gUID = System.Guid.NewGuid().ToString();
            //--▲▲▲ RFC 4122, Sec. 4.4에 설명된 대로 버전 4 UUID(유니버설 고유 식별자)를 만듭니다. 
            //-- https://learn.microsoft.com/ko-kr/dotnet/api/system.guid.newguid?view=net-8.0
        }
    }
}
