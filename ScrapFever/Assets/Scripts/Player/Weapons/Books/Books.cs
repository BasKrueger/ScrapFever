using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Books : AbstractWeapon
{
    [SerializeField, MinValue(0.1f), OnValueChanged("UpdateBookDistance"), TabGroup("Specifics")]
    private float bookRange = 4;

    [SerializeField, SuffixLabel("full spins per second", true), TabGroup("Specifics")]
    private float speed = 1;

    [SerializeField, FoldoutGroup("References")]
    private Animator booksAnim;

    private List<Book> books = new List<Book>();

    private void Start()
    {
        booksAnim = GetComponentInChildren<Animator>(true);

        foreach(var book in GetComponentsInChildren<Book>(true))
        {
            books.Add(book);
            book.SetUp(base.damage, this);
            book.Show();
        }

        booksAnim.gameObject.SetActive(true);
        booksAnim.SetFloat("speed", speed);
    }

    protected override void Fire(Transform target)
    {
        booksAnim.gameObject.SetActive(true);
    }

    private void UpdateBookDistance()
    {
        if (bookRange < 0.1f) return;

        foreach (var book in GetComponentsInChildren<Book>())
        {
            var delta = book.transform.position - transform.position;
            delta.Normalize();

            book.transform.position = transform.position + delta * (bookRange / 2);
        }
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>()
        {
            ("GlueBookRange"),
            ("GlueSpeed")
        };
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        upgrade.TryUpgradeSpecific(ref bookRange, "GlueBookRange");
        upgrade.TryUpgradeSpecific(ref speed, "GlueSpeed");

        foreach(var book in books)
        {
            book.SetUp(base.damage, this);
        }

        booksAnim.SetFloat("speed", speed);
        UpdateBookDistance();
    }

    private void OnDisable()
    {
        foreach (var book in books)
        {
            book.gameObject.SetActive(false);
        }
    }
}
