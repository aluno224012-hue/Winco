# Objetivos semanais — Winco

Atualizado em 19 nov 2026, a partir do que está no código (branch `main`, commit `3343310`, mais as alterações por commitar). Semanas 2 a 5 revistas depois de integrar o trabalho que faltava (ainda sem commit). Semanas 9 a 24 e pacotes de reserva copiados do plano semanal (ainda por fazer).

## Semana 1 — Ambiente e repositório

- [x] Solução `Winco.slnx` com Winco.App (WPF), Winco.Core, Winco.Platform, Winco.Core.Tests (xUnit) e WincoTestTarget
- [x] CsWin32 em Winco.Platform, com `AllowUnsafeBlocks` e `NativeMethods.txt`
- [x] Commit "Estrutura inicial" (`b70d6c0`)

## Semana 2 — Listar janelas

- [x] Record `WindowInfo` (Handle, Title, ClassName, ProcessId, ProcessName, ProcessPath) — `Winco.Core/Windows/WindowInfo.cs`
- [x] Enumerador `Wincoenumerador.GetTopLevelWindows` — filtra visíveis, sem dono, sem `WS_EX_TOOLWINDOW`, não cloaked e com título (`IsTopLevelWindow`)
- [x] Método `Describe(hwnd)` separado — `Wincoenumerador.Describe`, usado pelo `GetTopLevelWindows`
- [x] MainWindow com ListBox e botão Atualizar

## Semana 3 — Aplicar atributos

- [x] `DwmStyler` com `SetCorners`, `SetBorderColor`, `SetCaptionColor`, `SetCaptionTextColor`, `SetDarkTitleBar` e `SetBackdrop`, que devolvem `bool` sem lançar exceção
- [x] Teste unitário da conversão `#RRGGBB` → COLORREF — `ColorConversionTests` (4 casos)
- [x] `SetOpacity` e `ResetOpacity` no `DwmStyler`
- [x] `SetWindowPos` com `SWP_FRAMECHANGED` depois do modo escuro — dentro do `DwmStyler.SetDarkTitleBar`
- [x] WincoTestTarget com um formulário "Alvo de teste" — o `Form1` tem agora esse título e 400×300
- [x] Janela do Winco com os botões das ações + Repor e barra de estado — Modo escuro, Cantos retos, Borda vermelha, Barra azul, Texto branco, Mica, Opacidade 70% e Repor

## Semana 4 — Compatibilidade e decisão

- [ ] `docs/compatibilidade.md` (modelo criado; falta preencher com os testes)
- [ ] `docs/decisao-m1.md` (modelo criado; falta preencher com os testes)
- [ ] Tag `v0.1` (o repositório ainda não tem tags)

## Semana 5 — Modelo de regras

- [x] `RgbColor` (Parse, TryParse, ToString, ToColorRef) — `Winco.Core/Rules/RgbColor.cs`
- [x] Enums `CornerPreference` e `BackdropKind` em Winco.Core — `Winco.Core/Rules/`
- [x] Records `MatchCriteria`, `WindowStyle` e `Rule` — `Winco.Core/Rules/`
- [x] `RuleMatcher.FindMatch` — `Winco.Core/Rules/RuleMatcher.cs`
- [x] `RuleMatcherTests` (cerca de 15 testes) — `Winco.Core.Tests/TestesMatcherRegras.cs` (21 casos), mais `TestesRgbColor.cs`

## Semana 6 — Guardar e carregar

- [ ] `WincoConfig` e `Profile`
- [ ] Opções JSON (camelCase, enums como texto, conversor de `RgbColor`)
- [ ] `ConfigStore` com `Load` e `Save` (ficheiro temporário + troca)
- [ ] Testes do `ConfigStore`
- [ ] `tests/fixtures/config-v1.json`

## Semana 7 — Detetar janelas novas

- [ ] `WindowWatcher` com `SetWinEventHook` (SHOW, DESTROY, FOREGROUND)
- [ ] Delegate guardado num campo
- [ ] Callback que só enfileira (`Channel`)
- [ ] `Dispose` com `UnhookWinEvent`

## Semana 8 — Aplicar automaticamente

- [ ] Interface `IWindowStyler` em Winco.Core e `WindowStyler` em Winco.Platform
- [ ] `RuleEngine` com estado por HWND e `ApplyToExisting`
- [ ] Testes com `FakeWindowStyler`
- [ ] Interface provisória com DataGrid de regras
- [ ] Tag `v0.2`

## Semana 9 (30 nov a 6 dez) — Estrutura do editor em MVVM
- [ ] Winco.App/ViewModels/: MainViewModel (Rules, SelectedRule) e RuleViewModel (herda de ObservableValidator)
- [ ] Comandos NewRule, DeleteRule, MoveUp, MoveDown e Save
- [ ] MainWindow com duas colunas: lista de regras (CheckBox ativa, Nova, Apagar, ↑, ↓) à esquerda e editor à direita
- [ ] Campos Nome, Processo (obrigatório, termina em .exe), Caminho, Classe, Título contém
- [ ] ComboBox de cantos e de modo escuro (com "Não alterar")
- [ ] Verificação: criar, editar, reordenar e apagar regras só pela interface; o config.json reflete cada alteração

## Semana 10 (7 a 13 dez) — Cores, opacidade e escolher janela
- [ ] Winco.App/Controls/ColorPicker.xaml (Não alterar, 12 amostras, #RRGGBB, pré-visualização)
- [ ] Opacidade: CheckBox "Não alterar" e Slider de 20 a 100
- [ ] Fundo: ComboBox desativada abaixo da build 22621, com dica
- [ ] Botão "Escolher janela aberta" que preenche Processo e Classe
- [ ] Verificação: regra completa para o Paint sem escrever o nome do processo à mão

## Semana 11 (14 a 20 dez) — Aplicar ao guardar, repor e área de notificação
- [ ] RuleEngine.Reevaluate() chamado depois de cada Guardar
- [ ] Reset com DWMWCP_DEFAULT, DWMWA_COLOR_DEFAULT, DWMSBT_AUTO e estilo original da opacidade; mensagem para o modo escuro
- [ ] Carga de trabalho "Desenvolvimento de ambiente de trabalho com C++" no Visual Studio e leitura do ExplorerTAP do TranslucentTB
- [ ] Área de notificação com NotifyIcon (Abrir, Pausar regras, Sair); fechar a janela só a esconde
- [ ] Verificação: definição de concluído do marco 3 em três aplicações, com o Winco escondido

## Semana 12 (21 dez a 3 jan, férias) — Versão 0.3 e primeira janela sobreposta
- [ ] Gravar a demonstração do marco 3 (Win+Alt+R)
- [ ] Tag v0.3 e release
- [ ] OverlayService, primeiro passo: OverlayWindow transparente que deixa passar o clique (WS_EX_TRANSPARENT | WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE)
- [ ] Rever docs/diario.md e aplicar os cortes 1 a 3 se houver mais de uma semana de atraso
- [ ] Verificação: o retângulo de teste fica por cima das janelas e o clique passa através dele

## Semana 13 (4 a 10 jan) — Prova de injeção, só leitura
- [ ] Projeto Winco.Explorer (DLL C++/WinRT, x64)
- [ ] Classe COM com IObjectWithSite; em SetSite, IVisualTreeService3 e AdviseVisualTreeChange
- [ ] Escrever a árvore da barra em %LOCALAPPDATA%\Winco\arvore-barra.txt, sem alterar nada
- [ ] Exportar DllGetClassObject e DllCanUnloadNow
- [ ] Programa de teste em C# que chama InitializeXamlDiagnosticsEx no explorer.exe
- [ ] docs/decisao-m4.md até 10 jan: seguir com injeção ou voltar ao plano de outubro
- [ ] OverlayService: seguir uma janela (DWMWA_EXTENDED_FRAME_BOUNDS, EVENT_OBJECT_LOCATIONCHANGE, esconder ao minimizar, DPI por monitor)
- [ ] OverlayService: ficar logo acima do alvo na ordem das janelas (SetWindowPos com GW_HWNDPREV)
- [ ] Verificação: arvore-barra.txt mostra os elementos da barra e o Explorador nunca fechou

## Semana 14 (11 a 17 jan) — Mudar o fundo da barra
- [ ] Encontrar os elementos de fundo pelo tipo e nome, nunca pela posição
- [ ] Trocar o Fill por um SolidColorBrush e guardar o pincel original
- [ ] Evento Local\WincoTaskbarReset para repor
- [ ] try/catch(...) em cada função exportada e callback da DLL
- [ ] Verificação: a barra fica semitransparente, o evento repõe-na, terminar o explorer.exe volta ao normal

## Semana 15 (18 a 24 jan) — Ligar ao Winco
- [ ] TaskbarStyle em Winco.Core (Color, Opacity, HideBorder) com testes de serialização
- [ ] ExplorerInjector em Winco.Platform (Inject, IsInjected, Notify com taskbar.json e Local\WincoTaskbarChanged)
- [ ] Na DLL, thread que espera o evento e aplica a configuração
- [ ] Separador Barra de tarefas: módulo experimental, cor, opacidade, linha de borda e ocultar automático (SHAppBarMessage)
- [ ] Verificação: mudar a cor no Winco muda a barra em menos de 1 s

## Semana 16 (25 a 31 jan) — Proteção e versão 0.4
- [ ] CrashGuard em Winco.Core com testes (2 reinícios em 5 min desligam o módulo)
- [ ] Reinjetar com a mensagem TaskbarCreated
- [ ] Módulo desligado na primeira execução
- [ ] CapabilityService: taskbarModule com as builds testadas
- [ ] Testar na máquina virtual 26H2
- [ ] Tag v0.4 e release com GIF
- [ ] Verificação: definição de concluído do marco 4

## Semana 17 (1 a 7 fev) — Capacidades e tema
- [ ] ThemeSettings e DesktopSettings em Winco.Core/Config/
- [ ] CapabilityService com a tabela de funcionalidades e testes com as builds 19045, 22000, 22621 e 26100
- [ ] docs/registo-chaves.md com o método reg export / fc
- [ ] ThemeRegistry (AppsUseLightTheme, SystemUsesLightTheme) + WM_SETTINGCHANGE "ImmersiveColorSet"
- [ ] Cor de destaque só se sobrarem horas (limite 4 h)
- [ ] Separador Ambiente de trabalho com etiquetas "não documentado"
- [ ] Verificação: mudar o modo escuro no Winco muda o Explorador e as Definições sem reiniciar

## Semana 18 (8 a 14 fev) — Papel de parede e exportar
- [ ] Papel de parede com SystemParametersInfo(SPI_SETDESKWALLPAPER), igual em todos os monitores
- [ ] Copiar a imagem para %APPDATA%\Winco\wallpapers\
- [ ] ProfileExporter.Export: ZIP com profile.json e assets/wallpaper
- [ ] Menu Ficheiro → Exportar perfil…
- [ ] Teste do ZIP exportado (profile.json, imagem, schemaVersion: 1)
- [ ] Verificação: o papel de parede muda logo e o .winco abre como ZIP

## Semana 19 (15 a 21 fev) — Importar, validar e migrar
- [ ] ProfileImporter.Import com ImportResult (erros e avisos)
- [ ] Limite de 20 MB e proteção zip slip
- [ ] Recusar schemaVersion maior que a atual; migrações em cadeia (IMigration, Migrator)
- [ ] ProfileValidator (cores, .exe, opacidade 20–100) e ids novos
- [ ] Relatório de compatibilidade com o CapabilityService
- [ ] tests/fixtures/: profile-v1.winco, bad-color.winco, future-version.winco, zip-slip.winco
- [ ] Menu Ficheiro → Importar perfil… com o relatório
- [ ] Verificação: os quatro ficheiros de teste dão o resultado esperado

## Semana 20 (22 a 28 fev) — Arranque, registo de erros e versão 0.5
- [ ] "Iniciar com o Windows" em HKCU\...\Run com --minimized
- [ ] Instância única com Mutex Local\Winco
- [ ] FileLogger em %LOCALAPPDATA%\Winco\logs (apaga com mais de 7 dias)
- [ ] Exportar no PC principal e importar na máquina virtual
- [ ] Tag v0.5 e release
- [ ] Verificação: definição de concluído do marco 5; a partir de 1 mar não entram funcionalidades

## Semana 21 (1 a 7 mar) — Congelamento e matriz de testes
- [ ] Só correções; cada erro vira uma issue com a etiqueta bug
- [ ] docs/resultados-testes.md em três máquinas
- [ ] Windows 10 (ou teste do CapabilityService com 19045)
- [ ] Corrigir primeiro os erros do guião de demonstração
- [ ] Terminar o explorer.exe dez vezes com o módulo ativo e confirmar que o Winco reaplica
- [ ] Verificação: docs/resultados-testes.md preenchido

## Semana 22 (8 a 14 mar) — Robustez e medições
- [ ] Todos os métodos do Winco.Platform verificam o HRESULT e não deixam sair exceções
- [ ] Três manipuladores globais de exceções ligados ao registo
- [ ] Latência (50 aberturas do Bloco de Notas), carga (20 janelas) e consumo (10 min em repouso)
- [ ] Impacto do módulo da barra no explorer.exe, com e sem módulo
- [ ] docs/medicoes.md
- [ ] Verificação: a tabela de medições existe e o teste de carga passa

## Semana 23 (15 a 21 mar) — Instalador
- [ ] dotnet publish self-contained, sem ficheiro único nem trimming
- [ ] Inno Setup com PrivilegesRequired=lowest
- [ ] Testar no Windows Sandbox: instala, arranca, aplica uma regra, desinstala
- [ ] Decidir o que a desinstalação faz a %APPDATA%\Winco
- [ ] Incluir Winco.Explorer.dll (Release x64) no instalador
- [ ] Analisar com o Defender e o VirusTotal; submeter falsos positivos à Microsoft até 15 mar
- [ ] Verificação: instala numa máquina limpa sem pedir administrador

## Semana 24 (22 a 31 mar) — Demonstração e versão 1.0
- [ ] docs/guiao-demo.md com 8 passos para 5 minutos
- [ ] Passo do módulo da barra no guião (sai se falhar num ensaio)
- [ ] Correr o guião três vezes seguidas no portátil da defesa
- [ ] Capturas de ecrã e vídeo de 2 minutos
- [ ] README do GitHub com instalação, funcionalidades e limitações
- [ ] Tag v1.0 e release com o instalador
- [ ] Verificação: definição de concluído do marco 6

## Pacotes de reserva (sem data; entram quando uma semana acaba mais cedo, nunca depois de 1 mar)

### Pacote B — Bordas com efeitos (camada 2)
- [ ] BorderEffect em Winco.Core (glow, gradient, pulse; cores, espessura, velocidade)
- [ ] Border animado na OverlayWindow (gradiente, brilho, pulso; CornerRadius 8)
- [ ] Ligar ao RuleEngine (Attach/Detach), começando só pela janela em primeiro plano
- [ ] CornerRadius 0 em janelas com cantos retos
- [ ] Medir CPU/GPU com 1 e 10 janelas; limitar a 30 fps se passar de 5%
- [ ] Verificação: a borda segue a janela, some ao minimizar e fica tapada por janelas por cima (se não entrar até 31 jan, fica a cor fixa do DWM)

### Pacote C — Efeitos no clique (camada 3)
- [ ] Hook WH_MOUSE_LL no processo do Winco, delegate guardado num campo
- [ ] O hook só enfileira o ponto do clique
- [ ] OverlayWindow por monitor com a onda animada (300 ms)
- [ ] Desligar em ecrã inteiro (SHQueryUserNotificationState)
- [ ] Opções (ligar, cor, tamanho) guardadas no perfil
- [ ] Verificação: a onda aparece no ponto certo em todos os monitores e o clique chega à janela de baixo

### Pacote D — Widgets no ambiente de trabalho (camada 3)
- [ ] WidgetWindow base sempre no fundo (HWND_BOTTOM em WM_WINDOWPOSCHANGING)
- [ ] Widgets de relógio, CPU/memória e notas
- [ ] Posições guardadas em DesktopSettings.Widgets e restauradas no arranque
- [ ] Limite assumido: Win+D esconde os widgets
- [ ] Verificação: os widgets ficam atrás das janelas, voltam ao mesmo sítio e entram no .winco

### Pacote E — Barra lateral com pesquisa (camada 3)
- [ ] SideBarWindow registada como AppBar (ABM_NEW, ABM_QUERYPOS, ABM_SETPOS)
- [ ] ABM_REMOVE sempre ao fechar
- [ ] Pesquisa nos atalhos do menu Iniciar e abrir com Process.Start
- [ ] Atalho de teclado com RegisterHotKey
- [ ] Opções de lado, largura e ocultar automático
- [ ] Verificação: a barra fica colada à lateral, abre o Paint ao escrever "pai" e liberta o espaço ao fechar

### Pacote F — Vários perfis (camada 3)
- [ ] ProfileService (Create, Duplicate, Rename, Delete, Activate)
- [ ] Activate reaplica regras, widgets e barra lateral
- [ ] Testes do ProfileService
- [ ] Barra de perfis no topo da janela
- [ ] Verificação: alternar entre "Escola" e "Casa" muda tudo de uma vez

---

**Build e testes (19 nov 2026, .NET SDK 10.0.402, depois da integração):** Winco.Core, Winco.Platform, Winco.App e Winco.Core.Tests compilam com 0 erros (avisos NU1510 no Winco.App.csproj); `dotnet test` com 5 de 5 testes a passar. A solução inteira (`dotnet build Winco.slnx`) falha só no WincoTestTarget, com 10 erros no `Fulmorio.xaml.cs` (código WPF num projeto Windows Forms).
