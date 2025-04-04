using DG.Tweening;
using UnityEngine;

public class FoodShake : MonoBehaviour
{
    public bool isAnimating;
    public void FoodShaking(GameObject obj)
    {
        Vector3 initialSize = obj.transform.localScale;
        if (isAnimating) return;
        isAnimating = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScaleX(initialSize.x + 0.2f, 0.5f))
           .Join(obj.transform.DOScaleZ(initialSize.z + 0.2f, 0.5f))
           .Join(obj.transform.DOScaleY(initialSize.y - 0.2f, 0.5f))

           .Append(obj.transform.DOScaleX(initialSize.x, 0.5f).SetEase(Ease.OutElastic, 3f, 0.25f))
           .Join(obj.transform.DOScaleZ(initialSize.z, 0.5f))
           .Join(obj.transform.DOScaleY(initialSize.y, 0.5f).SetEase(Ease.OutElastic, 3f, 0.25f))
           .OnComplete(() => isAnimating = false);
    }
    
    

}
