# 🚀 SpaceShip — Guia de Configuração no Unity Editor

Os scripts já estão em `Assets/Scripts/` e foram subidos ao repositório. Siga os passos abaixo **na ordem exata**.

---

## 1. Preparar os Sprites

No **Project** panel, selecione cada sprite → aba **Inspector → Texture Type: Sprite (2D and UI)** → clique **Apply**:

Farback01.png, Farback02.png, Stars.png, Ship01.png, Ship02.png, Ship03.png, Ship04.png

---

## 2. Configurar a Câmera

- `Camera → Projection`: **Orthographic**
- `Camera → Size`: **5**

---

## 3. Montar o Background (Parallax)

Hierarquia (Background como filho de Main Camera):

```
Main Camera
└── Background (Empty)
    ├── Farback01_A  (X=0,     Sprite: Farback01)
    ├── Farback01_B  (X=10.24, Sprite: Farback01)
    ├── Farback02_A  (X=0,     Sprite: Farback02)
    ├── Farback02_B  (X=10.24, Sprite: Farback02)
    ├── Stars_A      (X=0,     Sprite: Stars)
    └── Stars_B      (X=10.24, Sprite: Stars)
```

Adicione Parallax.cs a cada sprite:

| Objeto | parallaxEffect | baseSpeed | Order in Layer |
|---|---|---|---|
| Farback01 A/B | 0.2 | 2 | 0 |
| Farback02 A/B | 0.4 | 2 | 1 |
| Stars A/B | 0.7 | 2 | 2 |

---

## 4. Prefab de Bala

1. Create Empty → `Bullet`
2. Sprite Renderer (sprite pequeno branco)
3. Rigidbody 2D: Gravity Scale = 0
4. Circle Collider 2D → Is Trigger = true
5. Bullet.cs: Speed = 10
6. Tag: `Bullet`
7. Arraste para `Assets/Prefabs/Bullet` → delete da cena

---

## 5. Prefabs de Inimigos (Ship02, Ship03, Ship04)

Para cada nave:

1. Create Empty → `EnemyShip02` etc.
2. Sprite Renderer → sprite da nave
3. Rigidbody 2D: Gravity Scale = 0
4. Box Collider 2D → Is Trigger = true
5. EnemyController.cs: Move Speed = 3, Score Value = 10
6. Tag: `Enemy`
7. Arraste para `Assets/Prefabs/` → delete da cena

---

## 6. Player (Ship01)

1. Create Empty → `Player`
2. Sprite Renderer → Ship01.png
3. Rigidbody 2D: Gravity Scale = 0, Collision Detection = Continuous
4. Box Collider 2D → Is Trigger = true
5. PlayerController.cs:
   - Crie filho Empty chamado `FirePoint` em X = 0.5
   - Bullet Prefab: prefab Bullet
   - Fire Point: FirePoint
   - Move Speed = 5, Fire Rate = 0.25, Slow Duration = 3
6. Tag: `Player`

---

## 7. EnemySpawner

1. Create Empty → `EnemySpawner`
2. EnemySpawner.cs:
   - Enemy Prefabs (size 3): arraste os 3 prefabs inimigos
   - Spawn Interval = 2, Min Y = -3.5, Max Y = 3.5

---

## 8. Canvas / UI

Canvas: Screen Space - Overlay, Scale With Screen Size, 1920x1080

```
Canvas
├── HUD
│   └── ScoreText (TextMeshPro - canto sup. esq., fonte 36)
├── GameOverPanel  (desativar por padrao)
│   ├── Text "GAME OVER"
│   └── Button "Reiniciar"
└── VictoryPanel   (desativar por padrao)
    ├── Text "YOU WIN!"
    └── Button "Reiniciar"
```

---

## 9. GameManager

1. Create Empty → `GameManager`
2. GameManager.cs - referencias:

| Campo | Objeto |
|---|---|
| Score Text | ScoreText (TMP) |
| Game Over Panel | GameOverPanel |
| Restart Button Lose | botao no GameOverPanel |
| Victory Panel | VictoryPanel |
| Restart Button Win | botao no VictoryPanel |
| Slow Motion Unlock Score | 10 |
| Victory Score | 50 |

---

## 10. Tags

Edit → Project Settings → Tags and Layers:
- `Player`, `Enemy`, `Bullet`

---

## 11. Sorting Layers

| Layer | Objetos |
|---|---|
| Background | Farback01/02 |
| Stars | Stars |
| Ships | Player, Enemies |
| Bullets | Bullet |

---

## 12. Controles para testar

| Tecla | Acao |
|---|---|
| WASD / Setas | Mover nave |
| Espaco | Atirar |
| Shift (apos 10 pts) | Slow-motion 3s |
| 50 pontos | Vitoria |
| Inimigo toca player | Game Over |
