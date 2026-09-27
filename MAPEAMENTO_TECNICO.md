# Mapeamento técnico do jogo

## Objetivo e escopo

Este documento registra o entendimento dos scripts em `Assets/_Scripts` e da cena `Assets/_Scenes/Inicio.unity`, para servir de referência durante atualizações e refatorações. O projeto foi atualizado para Unity 6.6. A primeira alteração de input migrou os controladores do jogo para leitura direta de dispositivos pelo Input System.

O jogo usa uma única cena com quatro áreas/níveis. A seleção não carrega cenas distintas: `InterfaceManager` escolhe uma posição inicial e reposiciona o jogador. A câmera acompanha o jogador. Este mapeamento foi feito por leitura dos scripts, do YAML da cena e do prefab `Assets/_Prefabs/Interface/Interface Manager.prefab`. A migração de input foi validada estaticamente, mas ainda não foi testada em Play Mode.

## Funcionamento provável

1. O jogador abre a seleção de nível e escolhe um dos quatro níveis.
2. `InterfaceManager` reposiciona o jogador, mostra a interface de jogo, inicia o cronômetro e zera a contagem de movimentos.
3. O jogador se desloca em passos unitários pelos eixos X/Z. Os comandos podem vir de WASD/setas ou das flags definidas pelo sistema de swipe.
4. Paredes com tag `Wall` bloqueiam o passo. Botões próximos são encontrados por raycasts e acionados através de `_ButtonLogic` e seus `UnityEvent`s configurados no Inspector.
5. Os eventos dos botões podem manipular os cenários. `_Scenario` pode abrir portas, desativar colisores e transformar o marcador de chão em saída (`Ganhou`). A configuração exata dos eventos determina os puzzles de cada área.
6. Blocos `Crackable` racham com o uso e podem cair; o jogador também pode cair e perder.
7. Alcançar um objeto com tag `Ganhou` vence o nível. Encontrar o jogador adjacente ao inimigo chama a derrota.
8. Reiniciar retorna à área inicial do nível 1 no fluxo-base, restaura cenários e blocos e, quando solicitado, inicia novamente o nível que estava selecionado.

Esse fluxo é uma interpretação do código. A montagem do Inspector, tags, colisores, prefabs e eventos pode alterar o comportamento efetivo.

## Mapa dos scripts

| Script | Responsabilidade observada | Ligações e observações |
|---|---|---|
| `MovementController.cs` | Movimento do jogador em passos, rotação, bloqueio por paredes, acionamento de botões, vitória, câmera e queda. | Lê WASD/setas por `Keyboard.current` e flags públicas; usa `InterfaceManager`, `ManagerDeScenario`, `CinemachineVirtualCameraBase` e `_ButtonLogic`. A base permite referenciar o componente de câmera atualmente serializado ou o novo `CinemachineCamera`. `gameplay` é atualizado pela interface, mas não é usado para interromper `Move()`. |
| `InterfaceManager.cs` | Menus, escolha dos níveis, estados de jogo, cronômetro, contador de movimentos, transições, FPS e reinício. | Mantém referências a jogador, inimigo, cenários, blocos, UI e câmera. O nível atual é usado na opção de jogar novamente. |
| `EnemyMovementController.cs` | Movimento discreto do inimigo e detecção de contato com o jogador. | Só se move quando suas flags direcionais são definidas. A lógica que escolheria movimentos a partir do contador está comentada; o campo `actualMoves` sem uso foi removido. |
| `_ButtonLogic.cs` | Executa um `UnityEvent` uma vez até o reset e anima o eixo do botão. | O efeito do botão depende dos listeners persistentes definidos no Inspector. |
| `_Scenario.cs` | Altera o estado da saída, ativa/desativa objetos e anima as portas do cenário. | Tem referências para botão, saída, colisores, chão e portas. O estado inicial das portas usa `isNormalOpen`. |
| `ManagerDeScenario.cs` | Coordena fechar e restaurar as portas de todos os cenários. | A cena contém quatro referências na lista `scenarios`, uma para cada cenário. |
| `_CrackBlock.cs` | Detecta o jogador sobre o bloco, remove a indicação de rachadura e, em ativação subsequente, inicia queda do bloco e do jogador. | Usa a contagem global de movimentos para permitir nova rachadura e referencia `MovementController` e `InterfaceManager`. |
| `CrackBlockManager.cs` | Restaura todos os blocos racháveis. | Mantém um array `_CrackBlock[]` atribuído por Inspector. |
| `_StatisticsManager.cs` | Guarda movimentos e tempo e contém rotina de gravação em `PlayerPrefs`. | O método de gravação é privado e não foi encontrado sendo chamado. Não aparenta fazer parte do fluxo atual. |
| `Swipe_1/SwipeController.cs` | Converte mouse/toque em tap, double tap e direção de swipe. | É o tipo referenciado por `SwipeManager`; lê `Mouse` e `Touchscreen` diretamente pelo Input System. |
| `Swipe_1/SwipeManager.cs` | Encaminha os swipes para as flags do jogador e do inimigo. | Para cima/baixo, o inimigo recebe a direção oposta; esquerda/direita também o move na direção oposta. |
| `Swipe_1/SwipeController_v2.cs` | Segunda implementação de detecção de gestos, com dead zone configurável. | Não foram encontradas referências a esta classe nos demais scripts pesquisados; aparenta estar desconectada. Também foi migrada para leitura direta de `Mouse` e `Touchscreen`, embora permaneça sem uso confirmado. |
| `SimpleCameraController.cs` | Controlador genérico de câmera livre por teclado e mouse, aparentemente baseado em template do Unity. | Agora lê `Keyboard` e `Mouse` diretamente pelo Input System; não foi encontrada ligação com o fluxo de câmera do jogador. |
| `CinemachineFolder/LockCameraY.cs` | Extensão Cinemachine que fixa a coordenada Y da posição da câmera. | Pode afetar uma Virtual Camera apenas se estiver anexada/configurada nela. O código da cena sozinho não confirma essa associação. |

## Cena e Inspector

- `Inicio.unity` contém objetos `ScenarioLevel1`, `ScenarioLevel2`, `ScenarioLevel3` e `ScenarioLevel4` e os objetos da UI/menu. Isso confirma a organização de quatro áreas dentro da mesma cena.
- Os botões da UI serializam chamadas para `Playlevel1`, `Playlevel2`, `PlayLevel3` e `PlayLevel4`.
- O `ScenarioManager` da cena lista quatro componentes `_Scenario`.
- A instância do prefab `Interface Manager` na cena tem overrides de referências para jogador, inimigo, menus, tela de jogo, telas de vitória/derrota, textos, gerenciador de blocos, `ManagerDeScenario` e Cinemachine.
- O `EventSystem` desse prefab usa `InputSystemUIInputModule` e o projeto está configurado com Input System como Active Input Handling. O módulo UI usa as ações padrão fornecidas pelo pacote; não foi criado um asset de Actions próprio.
- A troca de nível e o reinício usam uma única transição curta no `InterfaceManager`: fade da imagem atribuída a `img` até alpha 1, preparação do jogador/UI com a tela coberta, uma breve pausa opaca e fade até alpha 0. Cada fade dura 0,15 s, a pausa opaca 0,2 s, e a animação usa tempo não escalado sem alterar a cor RGB da imagem.
- Posições iniciais serializadas: nível 1 `(-0.014, 0, 105.061)`; nível 2 `(-38.684, 0, 105.061)`; nível 3 `(-73.149, 0, 105.061)`; nível 4 `(-100.16, 0, 105.07)`. Os valores de níveis 1 e 2 estão no prefab; a cena sobrescreve X/Z dos níveis 3 e 4.
- `MovementController.SetPositionStart()` configura o `Follow` da Virtual Camera para o jogador, define um offset fixo e invalida o estado anterior da câmera para que o teleporte de nível não passe pelo damping. O `CinemachineTransposer` da cena tem damping 1 em X/Y/Z, que continua valendo durante o movimento normal. Embora o `InterfaceManager` declare `level1CameraPosition` e `level1CameraRotation`, o código lido não usa esses campos para posicionamento por nível.
- Tags encontradas na cena incluem `Wall`, `Ground`, `Ganhou` e `Crackable`, coerentes com as consultas dos scripts.
- `ScenarioLevel1` tem referências serializadas para a saída e portas, `isNormalOpen` habilitado, mas arrays `buttons`, `collidersToOpen` e `groundsToActivate` vazios nesse componente específico. Isso não permite concluir como os demais cenários estão configurados.
- Os `UnityEvent`s dos botões incluem dados de chamadas persistentes na cena e overrides de prefab. Para validar completamente os efeitos, é necessário seguir os alvos e métodos configurados no Inspector de cada botão.

## Pontos a investigar antes de modificar

1. **Entrada e estado do jogo:** `MovementController.Move()` roda em todo `Update`, mesmo quando `InterfaceManager` define `gameplay = false`. Verificar se o movimento deveria ser bloqueado em menus, vitória e derrota.
2. **Sistema de input:** os controladores do jogo usam agora o Input System. Confirmar no Editor que a configuração de Active Input Handling foi reconhecida e testar teclado, mouse, toque e navegação/clique da UI nas plataformas-alvo.
3. **Swipe:** confirmar qual versão deve ser mantida. A versão ligada (`SwipeController`) e a versão `v2` têm dead zones diferentes; `v2` ainda não tem uso confirmado.
4. **Inimigo:** definir se deve se mover automaticamente por turno, apenas em resposta a swipe ou por outra regra. A escolha aleatória baseada em movimentos está comentada.
5. **Eventos dos puzzles:** documentar os listeners de cada `UnityEvent` e conferir quais cenários/portas/colisores cada botão controla.
6. **Blocos e derrota:** validar os casos de primeira rachadura, queda subsequente, reset e queda do jogador no Play Mode. O comportamento usa contador global de movimentos e coroutines.
7. **Vitória e estados:** verificar se a detecção repetida da saída e as coroutines de encerramento devem ser protegidas contra chamadas múltiplas; testar derrota por inimigo e por queda.
8. **Reinício por nível:** conferir se a posição padrão inicial no método `Reiniciar()` é intencional e se reiniciar mantém corretamente o nível selecionado.
9. **Persistência de estatísticas:** decidir se tempos e movimentos devem ser salvos. `_StatisticsManager` existe, mas não aparece conectado ao fluxo analisado.
10. **Câmera:** conferir a Virtual Camera, seu offset, extensão `LockCameraY` e se a câmera deve apenas seguir o jogador ou usar enquadramentos específicos por área.
11. **Refatoração estrutural:** depois de documentar o comportamento desejado e testar os quatro níveis, considerar separar estado de nível, entrada, regras de puzzle, UI e câmera. A cena única não precisa ser alterada apenas por não ser a prática preferida; avaliar custos e benefícios contra o projeto real.

## Sequência sugerida para futuras modificações

1. Validar referências e eventos dos quatro níveis no Inspector e executar cada fluxo em Play Mode.
2. Definir critérios de funcionamento para teclado, toque, inimigo, botões, blocos, vitória, derrota e reinício.
3. Corrigir primeiro bugs comportamentais confirmados com testes pequenos, sem reorganizar simultaneamente a cena.
4. Atualizar outras APIs de Unity/Cinemachine em alterações isoladas e verificáveis.
5. Só então planejar refatorações maiores, preservando o comportamento esperado dos quatro níveis.

## Limites deste mapeamento

O conteúdo registra o que foi possível inferir dos 14 scripts listados, da cena e do prefab do `Interface Manager`. Não substitui uma inspeção completa de todos os prefabs nem a execução no Editor. Overrides podem depender dos assets de origem, e os listeners de eventos precisam ser validados por alvo e método no Inspector para estabelecer toda a lógica dos puzzles. Os exemplos importados de TextMesh Pro em `Assets/_ProjectSettings` ainda contêm chamadas ao Legacy Input Manager; não fazem parte dos controles do jogo nem foram alterados nesta migração.