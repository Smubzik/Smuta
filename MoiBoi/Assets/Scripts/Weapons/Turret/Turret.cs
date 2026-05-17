using UnityEngine;

public class Turret : MonoBehaviour
{
    public static Turret Instance { get; private set; }

    public float _offset;
    private float _time;
    public float _startTime;

    public GameObject _bullet;
    public Transform _point, _point2;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + _offset);

        if (_time <= 0f)
        {
            if (Input.GetMouseButtonDown(0)) { 
                Instantiate(_bullet, _point.position, transform.rotation);
                Instantiate(_bullet, _point2.position, transform.rotation);
                _time = _startTime;
            }
        }
        else
        {
            _time -= Time.deltaTime;
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
