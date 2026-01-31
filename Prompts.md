# LLM Kullanım Dokümantasyonu

## Özet

| Bilgi | Değer |
|-------|-------|
| Toplam prompt sayısı | 6 |
| Kullanılan araçlar | ChatGPT |
| En çok yardım alınan konular | inspector binding aracı, mimari önerileri |
| Tahmini LLM ile kazanılan süre | 2 - 3 saat |

---

## Prompt 1: [Konu Başlığı]

**Araç:** [ChatGPT]
**Tarih/Saat:** 2026-01-31 14:00

**Prompt:**
```
bana unityde characterController componentini kullanan ve wasd ve mouse ile hareket eden bir player controller ver. first person bakış açılı olacak.
hareketler linner olmayacak, interpolate edilecek, yani yumuşak bir hareket istiyorum.
input için de unitynin yeni input sistemini kullan. gerekli aksiyonların script karşılıklarını nasıl set edeceğim göster.
```

**Alınan Cevap (Özet):**
<details>
<summary>----------> genişletip inceleyiniz <-----------</summary>

```cs
Harika — sana CharacterController + New Input System + yumuşatılmış (interpolated) FPS controller bırakıyorum. Bu sistem:

WASD → hareket

Mouse → bakış

Hareket ve kamera smooth / yumuşak

Gravity destekli

Unity New Input System uyumlu

FPS oyunları için “industry standard” yapıda

1️⃣ Input System Kurulumu
Package Manager

Window → Package Manager → Input System → Install

Input Actions Asset Oluştur

Right Click → Create → Input Actions
Adı: PlayerInputActions

Aç ve şunları ekle:

Action Map: Player
🎮 Move

Name: Move

Action Type: Value

Control Type: Vector2

Binding: 2D Vector Composite

Up → W

Down → S

Left → A

Right → D

🖱 Look

Name: Look

Action Type: Value

Control Type: Vector2

Binding:

Path → <Mouse>/delta

Generate C# Class

Sağ üstten:

Generate C# Class
Ad: PlayerInputActions

Kaydet

2️⃣ Unity Sahne Kurulumu
Player GameObject
Player
 ├─ Camera
 └─ CharacterController

CharacterController Ayarları
Setting	Value
Height	1.8
Center Y	0.9
Radius	0.3
3️⃣ FPS Player Controller Script
FPSController.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    private CharacterController controller;
    private PlayerInputActions input;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float smoothTime = 0.1f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 15f;
    public float lookSmoothTime = 0.05f;
    public float maxLookAngle = 80f;

    Vector2 moveInput;
    Vector2 lookInput;

    Vector2 currentMoveVelocity;
    Vector2 currentLookVelocity;

    float verticalVelocity;
    float cameraPitch;

    Vector2 smoothMove;
    Vector2 smoothLook;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Player.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        smoothMove = Vector2.SmoothDamp(
            smoothMove,
            moveInput,
            ref currentMoveVelocity,
            smoothTime
        );

        Vector3 move = transform.right * smoothMove.x +
                       transform.forward * smoothMove.y;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        smoothLook = Vector2.SmoothDamp(
            smoothLook,
            lookInput,
            ref currentLookVelocity,
            lookSmoothTime
        );

        float mouseX = smoothLook.x * mouseSensitivity;
        float mouseY = smoothLook.y * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
```

</details>



**Nasıl Kullandım:**
- [ ] Direkt kullandım (değişiklik yapmadan)
- [x] Adapte ettim (değişiklikler yaparak)
- [ ] Reddettim (kullanmadım)

**Açıklama:**
> bana hızlıca bir karakter kontrolcüsü vemesi ve default hareket değerleriyle zaman kaybetmemek için kullandım.

**Yapılan Değişiklikler (adapte ettiyseniz):**
> proje boyunca ihtiyaçlara göre eklemeler ve adaptasyonlar yaptım. inputların nereden okunacağını değiştirdim ayrıca değerler üzerinde ince ayarlar yaptım. ayrıca cx coding conventions'a göre düzeltmeler yaptım.

---

## Prompt 2: mimari önerileri

**Araç:** [ChatGPT]
**Tarih/Saat:** YYYY-MM-DD HH:MM

**Prompt:**
```
bana unity de temel düzeyde bir envanter sistemini mimarisi göster. liste kullanacak. IItems'dan itemi alacak.
```

**Alınan Cevap (Özet):**
```
kurallara göre herhangi bir hata luşmadan düzenlenen script
```

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> herhangi bir şekilde hata oluşuyor mu kontrol ettim. inspector atamalarını da kontrol ettikten sonra git diff ile değişen kısımları inceledim. sorun olmadığını görünce de doğrudan aktardım

---

## Prompt 3: mimari önerileri

**Araç:** [ChatGPT]
**Tarih/Saat:** YYYY-MM-DD HH:MM
**Prompt:**
```
kural pdf + güncellenmesini istediğim script 
script içeriğini değiştirmeden c# kurallarına uygun bir şekilde yeniden yaz
```

**Alınan Cevap (Özet):**
<details>

<summary>----------> genişletip inceleyiniz <-----------</summary>

Süper, sana temel ama doğru mimariye sahip, ileride büyütebileceğin bir Unity envanter yapısı göstereyim. Liste kullanacağız ve item’lar IItem interface’i üzerinden yönetilecek. Bu yapı hem oyun mantığına hem UI’ye temiz bağlanır.

1. Temel Mimari Şema
[World Item]
    ↓ (pickup)
[IItem Interface]
    ↓
[Inventory]
    - List<IItem>
    ↓
[InventoryUI] (opsiyonel)


Amaç:
Inventory hiçbir zaman “kılıç, iksir, silah” bilmez.
Sadece IItem bilir.
Bu sayede sistem genişletilebilir olur.

2. IItem Interface

Bu, envantere girebilecek HER ŞEYİN kontratı:

public interface IItem
{
    string ItemID { get; }
    string DisplayName { get; }
    Sprite Icon { get; }

    void OnUse();
}


İstersen ileride şunları ekleyebilirsin:

Stackable mı?

MaxStack

Weight

Value

3. Örnek Item Implementasyonu

Mesela bir “Health Potion”:

using UnityEngine;

[CreateAssetMenu(menuName = "Items/Health Potion")]
public class HealthPotion : ScriptableObject, IItem
{
    [SerializeField] private string itemID;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int healAmount = 25;

    public string ItemID => itemID;
    public string DisplayName => displayName;
    public Sprite Icon => icon;

    public void OnUse()
    {
        Debug.Log($"{displayName} used. Healed {healAmount} HP");
    }
}


ScriptableObject kullanmamızın sebebi:
Item verisi sahnede değil, asset olarak yaşar.
Clean, reusable ve save/load dostu olur.

4. Inventory Sınıfı (Liste Kullanan Çekirdek Sistem)
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSize = 20;

    private List<IItem> items = new List<IItem>();

    public IReadOnlyList<IItem> Items => items;

    public bool AddItem(IItem item)
    {
        if (items.Count >= maxSize)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        items.Add(item);
        Debug.Log($"Added: {item.DisplayName}");
        return true;
    }

    public bool RemoveItem(IItem item)
    {
        if (items.Remove(item))
        {
            Debug.Log($"Removed: {item.DisplayName}");
            return true;
        }
        return false;
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= items.Count)
            return;

        items[index].OnUse();
    }
}

Buradaki mimari güzellik:

List<IItem> kullanıyoruz

Inventory, item türünü ASLA umursamıyor

Sadece OnUse() çağırıyor
</details>

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> sadece bir fikir edinme amaçlı yazdığım bir prompt. sınıfları kopyala yapıştır yaptıktan sonra kendi sistemime göre adapte ettim.
> event yapısı ekleyip UI ile beraber çalışmasını sağladım. objenin sahnedeki görünürlülüğünü ayarlamak için eklemeler yaptım.

---


## Prompt 4: hızlı implementasyon promptları

**Araç:** [ChatGPT]
**Tarih/Saat:** YYYY-MM-DD HH:MM
**Prompt:**
```
bana update içinde çalıacak basit bir timer ver. bir bool ile aktif olacak ve bir float duration değeri olacak bittiğinde istediğim fonksiyonu çağıracak.
```

**Alınan Cevap (Özet):**
<details>

<summary>----------> genişletip inceleyiniz <-----------</summary>

Tabii, en sade ve “Update içinde çalışan” haliyle bir yapı bırakıyorum 👇
Bool ile aktif oluyor, duration kadar sayıyor, bitince çağırıyor ve kendini kapatıyor.

public class SimpleTimer : MonoBehaviour
{
    public bool timerActive = false;
    public float duration = 2f;

    private float timer;

    private IInteractable m_CurrentInteractable;

    void Update()
    {
        if (!timerActive)
            return;

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            timerActive = false;
            timer = 0f;

            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.StartInteractObject();
            }
        }
    }

    // Timer'ı dışarıdan başlatmak için
    public void StartTimer(IInteractable interactable, float newDuration)
    {
        m_CurrentInteractable = interactable;
        duration = newDuration;
        timer = 0f;
        timerActive = true;
    }
}

Kullanım örneği
simpleTimer.StartTimer(targetInteractable, 1.5f);
</details>

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> hızlıca bazı ufak kısımları implemente etmek için kullanıdım.

---



## Prompt 5: hatalı binding scripti

**Araç:** [ChatGPT]
**Tarih/Saat:** YYYY-MM-DD HH:MM
**Prompt:**
```
burada editör zamanında ve runtime'da bind işlemi sorunlu çalışıyor. ya state değişimi sırasında save kayboluyor yada birden fazla binding oluşuyor. 
```

**Alınan Cevap (Özet):**
```
çözülmeye çalışımış script. halen hataları var.
```

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> aldığım davranışsal hataları düzeltmek için kullandım
> sorunsuz yapıya ulaşmak için uzunca konuştuk.

---


## Prompt 6: unuttuğum yapıları hatırlama

**Araç:** [ChatGPT]
**Tarih/Saat:** YYYY-MM-DD HH:MM
**Prompt:**
```
unity de bir monobehaviour componentimde inspecto'dan fonksiyon nasıl alırım. unity eventleri gibi.

```

**Alınan Cevap (Özet):**
<details>

<summary>----------> genişletip inceleyiniz <-----------</summary>

Kısaca: UnityEvent kullanman gerekiyor. Unity’nin inspector’da “sürükle-bırak + fonksiyon seç” sistemi tamamen bunun üstüne kurulu.

Aşağıda sana buton, etkileşim, tetik vs. için birebir kullanılan mimariyi gösteriyorum.

1️⃣ En Basit ve Doğru Yöntem — UnityEvent
Script
using UnityEngine;
using UnityEngine.Events;

public class FunctionReceiver : MonoBehaviour
{
    [Header("Inspector'dan fonksiyon bağla")]
    [SerializeField] private UnityEvent onTriggered;

    public void Trigger()
    {
        Debug.Log("Trigger çağrıldı");
        onTriggered?.Invoke();
    }
}
</details>

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> uzun süredir kullanmadığım yapıları tekrar kullanamk için

---




## Genel Değerlendirme

### LLM'in En Çok Yardımcı Olduğu Alanlar
1. mimariyi planlama.
2. key binding ve sorunlarını çözme.
3. basit yapıları hızlıca implemente etme.
4. unuttuğum ve uzun süre kullanmadığım bazı yapıları tekrar hatırlamama yardım etti.

### LLM'in Yetersiz Kaldığı Alanlar
1. key binding sistemi ile uğraşırken güncel unity kütüphanesinde olmayan fonksiyonları kullandı.
2. key binding sistemini tasarlarken verdiğim komutları eksik yada hatalı tamamladı.

### LLM Kullanımı Hakkında Düşüncelerim
> Bu case boyunca LLM kullanarak neler öğrendiniz?
- key binding için bazı edge case'leri öğrendim.
- unuttuğum ve uzun süre kullanmadığım bazı yapıları tekrar hatırladım
> LLM'siz ne kadar sürede bitirebilirdiniz?
- kesinlikle 12 saatten uzun sürerdi
> Gelecekte LLM'i nasıl daha etkili kullanabilirsiniz?
- doğrudan ide içine entegre sistemler ile çalışarak aradaki zaman kaybını azaltabilirim diye düşünüyorum.
---
