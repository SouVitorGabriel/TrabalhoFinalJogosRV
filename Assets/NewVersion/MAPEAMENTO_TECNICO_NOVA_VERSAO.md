# Mapeamento Tecnico de Construcao da Nova Versao

## Escopo

Este documento consolida os achados e recomendacoes para evolucao da nova versao do projeto em paralelo ao legado, com foco em arquitetura, desacoplamento, modernizacao, simplificacao, performance e velocidade para criacao de novos levels.

## Achados Prioritarios

1. Build Settings aponta para uma cena que nao existe mais.
- Arquivo: ProjectSettings/EditorBuildSettings.asset
- Estado observado: caminho ativo em `Assets/LegacyVersion/_Scenes/Inicio.unity`, enquanto no workspace existem `Assets/ClassicVersion/_Scenes/Inicio.unity` e `Assets/NewVersion/InicioNew.unity`.
- Impacto: risco de build quebrado e roteamento incorreto de versoes.

2. A cena nova ainda esta fortemente acoplada a assets da versao classica.
- Arquivo: Assets/NewVersion/InicioNew2.unity
- Estado observado: referencias ao prefab de interface localizado em `Assets/ClassicVersion/_Prefabs/Interface/Interface Manager.prefab`.
- Impacto: alteracoes no legado podem afetar a nova versao sem isolamento.

3. Alto acoplamento entre UI e gameplay.
- Arquivos: Assets/ClassicVersion/_Scripts/InterfaceManager.cs e Assets/ClassicVersion/_Scripts/MovementController.cs
- Estado observado: o estado do gameplay e escrito pela UI enquanto o movimento roda continuamente em Update.
- Impacto: manutencao dificil e maior chance de regressao em transicoes/menu.

4. Duplicacao de logica de movimento entre player e inimigo.
- Arquivos: Assets/ClassicVersion/_Scripts/MovementController.cs e Assets/ClassicVersion/_Scripts/EnemyMovementController.cs
- Estado observado: estrutura semelhante de direcao, validacao por raycast e deslocamento.
- Impacto: cada ajuste exige manutencao em mais de um lugar.

5. Mecanicas dependem de polling frequente e tags string hardcoded.
- Arquivos: Assets/ClassicVersion/_Scripts/_CrackBlock.cs e Assets/ClassicVersion/_Scripts/MovementController.cs
- Estado observado: regras por contador global em Update, multiplos raycasts e comparacoes por string de tag.
- Impacto: custo de manutencao alto, risco de erro por typo e dificuldade de extensao.

6. Ruido operacional no runtime e codigo ocioso.
- Arquivos: Assets/ClassicVersion/_Scripts/InterfaceManager.cs, Assets/ClassicVersion/_Scripts/ManagerDeScenario.cs, Assets/ClassicVersion/_Scripts/_Scenario.cs, Assets/ClassicVersion/_Scripts/_StatisticsManager.cs
- Estado observado: logs por frame, Start/Update vazios e metodos de estatistica desconectados do fluxo.
- Impacto: menor legibilidade e menor confiabilidade para evolucao rapida.

## Recomendacoes de Arquitetura para a Nova Versao

1. Isolamento real entre Classic e New
- Congelar ClassicVersion.
- Duplicar para NewVersion apenas os prefabs/sistemas que realmente serao alterados.
- Garantir que a cena da NewVersion nao dependa de prefabs centrais da ClassicVersion.
- Criar fluxo de menu para selecao de versao e carregamento de cena especifica por versao.

2. Nucleo orientado a dados para levels
- Criar `LevelDefinition` (ScriptableObject) com:
  - spawn de player
  - spawn de inimigo
  - parametros de camera
  - lista de mecanicas ativas
  - condicao de vitoria
- Substituir metodos fixos por nivel por um carregador unico baseado em definicao de level.

3. Mecanicas plugaveis
- Criar contrato simples para mecanicas (exemplo: ciclo de vida de inicio de level, passo concluido e reset).
- Implementar mecanicas como modulos independentes:
  - movimento base
  - bloco rachado
  - botao/porta
  - inimigo espelhado
- Cada level ativa apenas os modulos necessarios.

4. Separar Input da regra de movimento
- Input vira produtor de comandos (direcao/intencao).
- Motor de movimento em grid executa comando e publica evento de passo concluido.
- Swipe, teclado e IA passam a ser fontes de comando reaproveitaveis.

5. Maquina de estados global do jogo
- Estados sugeridos: Menu, Transition, Playing, Won, Lost, Resetting.
- UI observa estado e gameplay executa apenas em Playing.
- Reduz conflitos entre coroutines, menu e controle do jogador.

## Boas Praticas de Acoplamento e Abstracao

1. Substituir comparacao de tag por string por `CompareTag` e, onde fizer sentido, `LayerMask`.
2. Centralizar vitoria/derrota/contagem de movimentos em um servico de sessao de level.
3. Reduzir referencias cruzadas diretas entre MonoBehaviours com eventos e interfaces.
4. Extrair um `GridMover` compartilhado para eliminar duplicacao entre player e inimigo.
5. Parametrizar valores hardcoded (spawn, tempos, distancias de raycast, offsets) em assets de configuracao.

## Modernizacao e Simplificacao

1. Criar asmdefs por contexto (Core, Mechanics, UI, Input, Classic) para reduzir compilacao e dependencias acidentais.
2. Consolidar animacoes temporais repetidas com utilitario comum de tween/coroutine.
3. Limpar scripts nao utilizados na linha New para reduzir ruida e acoplamento historico.
4. Manter documentacao tecnica sincronizada com os novos caminhos de pasta e responsabilidades.

## Otimizacoes Sugeridas

1. Remover logs em Update e Debug.DrawRay continuo fora de ambiente de debug.
2. Reduzir RaycastAll quando Raycast simples atender ao caso.
3. Avaliar APIs NonAlloc nas consultas fisicas frequentes.
4. Remover Start/Update vazios para limpar ciclo de frame e leitura do codigo.

## Plano de Execucao Sugerido

1. Corrigir roteamento de versoes e cenas (Build Settings e menu de selecao de versao).
2. Congelar ClassicVersion e isolar NewVersion por duplicacao controlada dos ativos centrais.
3. Extrair motor de movimento em grid e camada de comando de input.
4. Introduzir `LevelDefinition` e migrar os 4 levels atuais para pipeline orientado a dados.
5. Modularizar mecanicas por componentes plugaveis e validar regressao level a level.
6. A partir da base modular, criar novos levels sem alterar codigo central, apenas dados e composicao de mecanicas.

## Observacao Final

A proposta acima preserva o legado funcional enquanto cria uma trilha de evolucao segura para a nova versao, reduzindo risco de regressao e acelerando expansao de conteudo.

## Status Atual da Fatia 1

Implementacao inicial criada em `Assets/NewVersion/_Scripts` com foco em manter compatibilidade com o legado e iniciar a migracao orientada a dados.

Scripts adicionados:
- `Core/NvGameState.cs`
- `Core/NvGameFlowController.cs`
- `Levels/NvLevelDefinition.cs`
- `Levels/NvLevelCatalog.cs`
- `LegacyBridge/NvLegacyLevelRunnerAdapter.cs`

Objetivo desta etapa:
- Introduzir um fluxo novo de estado do jogo sem remover o fluxo legado.
- Introduzir definicao de level por ScriptableObject.
- Encaminhar execucao de level para o sistema atual via adaptador, sem quebra de comportamento.

## Checklist de Integracao no Inspector (Fatia 1)

1. Criar assets de dados:
- Criar 4 assets `NvLevelDefinition` (um por level), com `legacyLevelSlot` = 1, 2, 3 e 4.
- Criar 1 asset `NvLevelCatalog` e preencher a lista `levels` na ordem dos levels.

2. Configurar objeto de fluxo na cena New:
- Em `InicioNew2.unity`, criar um GameObject vazio chamado `NvGameBootstrap`.
- Adicionar os componentes `NvLegacyLevelRunnerAdapter` e `NvGameFlowController`.

3. Ligar referencias:
- Em `NvLegacyLevelRunnerAdapter`, atribuir `legacyInterfaceManager` com o objeto que ja contem `InterfaceManager` na cena.
- Em `NvGameFlowController`, atribuir `levelCatalog` com o asset criado.
- Em `NvGameFlowController`, atribuir `legacyRunner` com o componente `NvLegacyLevelRunnerAdapter` do mesmo objeto.

4. Ligar UI para comecar a usar o novo fluxo:
- No botao de abrir selecao de levels, chamar `NvGameFlowController.OpenLevelSelection()`.
- Nos botoes de level, chamar `NvGameFlowController.PlayLevel1/2/3/4()`.
- No botao de voltar menu, chamar `NvGameFlowController.ReturnToMainMenu()`.

5. Teste minimo de aceite:
- Abrir menu principal.
- Entrar na selecao de level.
- Iniciar level 1 pelo novo fluxo.
- Confirmar que a jogabilidade atual continua funcionando.
- Repetir para level 2, 3 e 4.

Observacao:
- Nesta Fatia 1, o movimento em grid e mecanicas ainda executam pelo codigo legado.
- O objetivo foi criar base nova de orquestracao e dados para permitir migracao progressiva nas proximas fatias.

## Status Atual da Fatia 2

Fatia 2 implementada com foco em migracao gradual da UI e preparacao de controle por level sem reescrever a cena inteira.

Alteracoes de codigo:
- `NvGameFlowController` agora possui aliases legados (`Playlevel1`, `Playlevel2`, `_TurnOnLevelSelection`, `Reiniciar(int mode)`), permitindo redirecionar callbacks antigos sem refatorar todos os botoes de uma vez.
- `NvGameFlowController` agora suporta selecao por `levelId` (`PlayLevelById`) e controle opcional de visibilidade dos grupos de level.
- `NvLevelCatalog` recebeu busca por `levelId` (`TryGetLevelById`).
- Novo script: `Scene/NvSceneLevelVisibilityController.cs` para mostrar apenas o level ativo na cena (ou ocultar/exibir todos), mantendo um conjunto de roots sempre visiveis.

Objetivo desta etapa:
- Consolidar a migracao da UI para o controlador novo sem quebra do fluxo legado.
- Preparar o projeto para manter os levels na mesma cena durante a transicao, com opcao de exibir apenas o level selecionado.

## Checklist de Integracao no Inspector (Fatia 2)

1. No objeto `NvGameBootstrap`:
- Em `NvGameFlowController`, manter `levelCatalog` e `legacyRunner` atribuidos.
- Recomendado: adicionar o componente `NvSceneLevelVisibilityController` no proprio objeto `NvGameBootstrap`.
- Alternativa valida: colocar `NvSceneLevelVisibilityController` em outro objeto (ex.: `NvSceneVisibility`) e arrastar a referencia para o campo `sceneVisibility` no `NvGameFlowController`.
- Regra pratica: se o campo `sceneVisibility` do `NvGameFlowController` estiver preenchido, funciona em qualquer objeto; usar o proprio `NvGameBootstrap` apenas simplifica manutencao.

2. Configurar grupos de cena no `NvSceneLevelVisibilityController`:
- Criar um grupo por `levelId` (mesmo valor usado no `NvLevelDefinition`).
- Em cada grupo, incluir os roots do level correspondente (ex.: `Level1` + `ScenarioLevel1`, etc.).
- Em `alwaysVisibleRoots`, incluir objetos globais que devem ficar sempre ativos (ex.: `Canvas`, `Main Camera`, `Interface Manager`, `NvGameBootstrap`, `Directional Light`).

3. Ativar controle de visibilidade no fluxo novo:
- Marcar `controlSceneVisibility` no `NvGameFlowController` para habilitar a troca visual por level.
- Com `controlSceneVisibility` ligado, ao clicar em Play de um level o sistema tenta ativar somente o grupo cujo `levelId` bate com o `NvLevelDefinition` selecionado.
- Definir `hideGameplayGroupsOnMainMenu`:
  - `true`: ao voltar ao menu, grupos de gameplay ficam ocultos e ficam visiveis apenas os roots globais.
  - `false`: ao voltar ao menu, todos os grupos de gameplay podem permanecer visiveis.
- Essa etapa nao muda a regra de jogo; ela apenas controla quais objetos da cena ficam ativos visualmente.

4. Migrar callbacks restantes de Reiniciar para o fluxo novo:
- Onde ainda estiver `InterfaceManager.Reiniciar`, apontar para `NvGameFlowController.Reiniciar(int mode)` preservando os parametros atuais (0, 1 ou 2).
- Semantica atual no controlador novo:
  - `mode = 2`: replay do level atual (`ReplayCurrentLevel`).
  - `mode = 0` ou `mode = 1`: retorno ao menu principal (`ReturnToMainMenu`).
- Se voce quiser diferenciar `mode = 0` e `mode = 1` no futuro, isso pode ser ajustado sem alterar os botoes novamente.

5. Teste minimo de aceite:
- Abrir menu principal.
- Selecionar level 1, 2, 3 e 4.
- Confirmar que apenas o grupo do level ativo aparece (se `controlSceneVisibility` estiver ativo).
- Reiniciar no lose/win e voltar ao menu sem regressao de fluxo.

Detalhe importante sobre ScriptableObject x GameObjects de cena:
- O `NvLevelDefinition` (ScriptableObject) define dados e identidade do level (`levelId`, slot legado, etc.).
- O `NvSceneLevelVisibilityController` recebe GameObjects porque, nesta fase, os levels ainda existem fisicamente na mesma cena.
- A ligacao entre os dois mundos e o `levelId`: ao escolher um `NvLevelDefinition`, o sistema procura um grupo de cena com o mesmo `levelId` para ativar/desativar roots.
- Portanto, nao ha conflito: ScriptableObject define "qual level", roots de cena definem "quais objetos devem aparecer".

Observacao:
- A Fatia 2 ainda mantem execucao das mecanicas principais no codigo legado; o ganho aqui e de orquestracao, isolamento e preparo para a migracao da logica de turno/movimento nas proximas fatias.

## Status Atual da Fatia 3

Fatia 3 implementada com foco em iniciar o fluxo de turno em arquitetura nova, ainda usando os controllers legados como destino final do comando.

Alteracoes de codigo:
- Novo enum de direcao: `Turns/NvTurnDirection.cs`.
- Novo roteador de comando por turno: `Turns/NvTurnInputRouter.cs`.
- Nova ponte para envio de turno ao legado: `LegacyBridge/NvLegacyTurnBridge.cs`.
- Ajuste de transicao no legado para evitar dupla leitura de teclado na cena New: `MovementController.readKeyboardInput` (default `true`, preservando a Classic).

Objetivo desta etapa:
- Separar origem de input (teclado/swipe/UI) da execucao de movimento.
- Permitir evolucao para turno real no nucleo novo sem quebrar os levels atuais.

## Checklist de Integracao no Inspector (Fatia 3)

1. Configurar a ponte de turno no `NvGameBootstrap`:
- Adicionar o componente `NvLegacyTurnBridge` no `NvGameBootstrap`.
- Em `NvLegacyTurnBridge`, atribuir:
  - `player`: objeto `Player` (com `MovementController`).
  - `enemy`: objeto `Enemy` (com `EnemyMovementController`).
- Em `NvLegacyTurnBridge`, manter `driveEnemy = true` para replicar comportamento atual de levels com inimigo.

2. Configurar roteador de turno no `NvGameBootstrap`:
- Adicionar o componente `NvTurnInputRouter` no `NvGameBootstrap`.
- Em `NvTurnInputRouter`, atribuir:
  - `flowController`: componente `NvGameFlowController` do mesmo objeto.
  - `legacyTurnBridge`: componente `NvLegacyTurnBridge` do mesmo objeto.

3. Definir fontes de input na New:
- Para iniciar sem risco de duplicidade:
  - `readKeyboardInput = true` no `NvTurnInputRouter`.
  - `readSwipeInput = true` no `NvTurnInputRouter`.
- Ajustar `minTurnInterval` para `0.20` a `0.25` (recomendado: `0.22`).

4. Evitar comando duplicado entre novo e legado:
- No objeto `Player` (componente `MovementController`), desmarcar `readKeyboardInput` na cena New.
- Isso desativa leitura direta de teclado no legado apenas na New, mantendo a Classic intacta por default.

5. Ajuste de swipe legado na New (opcional, recomendado):
- Se o objeto `swipeManager` ainda estiver ativo, desabilitar apenas o componente `SwipeManager`.
- Se quiser manter o `SwipeController` antigo para testes comparativos, deixe-o ativo; caso contrario, pode desabilitar o objeto inteiro quando o novo roteador estiver validado.

6. Teste minimo de aceite:
- Entrar no level 1.
- Mover com teclado e confirmar um comando por turno (sem input duplo).
- Repetir no level 2 e confirmar que reset/volta menu continuam sem exceptions.
- No level 4, confirmar que o inimigo continua respondendo ao comando pelo perfil legado.

7. Checkpoint de seguranca:
- Confirmar que a cena Classic continua com comportamento original (na Classic, `MovementController.readKeyboardInput` permanece `true` por default).