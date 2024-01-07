using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CombatEnemyHurtController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private EnemyCombatModel model;

    public Material normalMaterial;
    public Material blinkMaterial;

    public int combo = 0;

    public Animator animator;

    private void Awake()
    {
        model = GetComponentInParent<EnemyCombatModel>();
    }

    public async void Hurt()
    {
        if (!model.canHit) return;

        StopCoroutine("RestartCombo");
        combo++;

        if (combo == 3)
            KnockBack();

        StartCoroutine("RestartCombo");

        spriteRenderer.material = blinkMaterial;
        await Task.Delay(100);
        spriteRenderer.material = normalMaterial;
    }

    private async void KnockBack()
    {
        model.canHit = false;

        animator.SetTrigger("knock");
        combo = 0;

        await Task.Delay(2000);

        animator.SetTrigger("getUp");

        await Task.Delay(300);

        model.canHit = true;
    }

    private IEnumerator RestartCombo()
    {
        yield return new WaitForSeconds(3);

        combo = 0;
    }
}
