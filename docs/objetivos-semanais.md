# Objetivos semanais — Winco

Atualizado em 19 nov 2026, a partir do que está no código (branch `main`, commit `3343310`, mais as alterações por commitar).

## Semana 1 — Ambiente e repositório

- [x] Solução `Winco.slnx` com Winco.App (WPF), Winco.Core, Winco.Platform, Winco.Core.Tests (xUnit) e WincoTestTarget
- [x] CsWin32 em Winco.Platform, com `AllowUnsafeBlocks` e `NativeMethods.txt`
- [x] Commit "Estrutura inicial" (`b70d6c0`)

## Semana 2 — Listar janelas

- [x] Record `WindowInfo` (Handle, Title, ClassName, ProcessId, ProcessName, ProcessPath) — `Winco.Core/Windows/WindowInfo.cs`
- [x] Enumerador `Wincoenumerador.GetTopLevelWindows` — filtra visíveis, sem dono, sem `WS_EX_TOOLWINDOW`, não cloaked e com título (`IsTopLevelWindow`)
- [ ] Método `Describe(hwnd)` separado (parcial: o `WindowInfo` é montado dentro do callback do `EnumWindows`; falta passá-lo para um método próprio)
- [x] MainWindow com ListBox e botão Atualizar

## Semana 3 — Aplicar atributos

- [x] `DwmStyler` com `SetCorners`, `SetBorderColor`, `SetCaptionColor`, `SetCaptionTextColor`, `SetDarkTitleBar` e `SetBackdrop`, que devolvem `bool` sem lançar exceção
- [x] Teste unitário da conversão `#RRGGBB` → COLORREF — `ColorConversionTests` (4 casos)
- [x] `SetOpacity` e `ResetOpacity` no `DwmStyler`
- [ ] `SetWindowPos` com `SWP_FRAMECHANGED` depois do modo escuro (está no `NativeMethods.txt`, mas ainda não é chamado)
- [ ] WincoTestTarget com um formulário "Alvo de teste" (parcial: abre o `Form1`, que ainda tem o título "Form1"; o `Fulmorio.xaml` com "Alvo de teste" era uma cópia da MainWindow e está agora vazio)
- [ ] Janela do Winco com os botões das ações + Repor e barra de estado (parcial: tem Modo escuro, Cantos retos, Borda vermelha, Barra azul, Repor e a barra de estado; faltam botões para a cor do texto do título, o backdrop e a opacidade)

## Semana 4 — Compatibilidade e decisão

- [ ] `docs/compatibilidade.md`
- [ ] `docs/decisao-m1.md`
- [ ] Tag `v0.1` (o repositório ainda não tem tags)

## Semana 5 — Modelo de regras

- [ ] `RgbColor` (Parse, ToString, ToColorRef) em Winco.Core — por agora só existe `ColorConversion.HexToColorRef`
- [ ] Enums `CornerPreference` e `BackdropKind` em Winco.Core — por agora existem `CornerStyle` e `BackdropStyle`, mas em Winco.Platform
- [ ] Records `MatchCriteria`, `WindowStyle` e `Rule`
- [ ] `RuleMatcher.FindMatch`
- [ ] `RuleMatcherTests` (cerca de 15 testes)

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

---

**Build e testes (19 nov 2026, .NET SDK 10.0.402):** `dotnet build` concluído com 0 erros e 4 avisos (NU1510: `System.Memory` e `System.Runtime.CompilerServices.Unsafe` desnecessários no Winco.App.csproj); `dotnet test` com 5 de 5 testes a passar (4 da conversão de cor + o `UnitTest1.Test1` vazio).
