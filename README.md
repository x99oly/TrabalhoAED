# ✅ Progresso do Projeto - Gerenciador de Processo Seletivo da Universidade Stark

## ✅ Funcionalidades já implementadas

### 📄 Leitura do Arquivo `entrada.txt`
- [x] A primeira linha do arquivo (`qtdCursos;qtdCandidatos`) é lida e processada.
- [x] As linhas seguintes são corretamente interpretadas para:
  - [x] Criar instâncias de `Course` com `Id`, `Nome` e `Vagas`.
  - [x] Criar instâncias de `Applicant` com `Nome`, Notas (Redação, Matemática, Linguagens), e opções de curso (`Op1`, `Op2`).

### 📚 Estrutura de Dados
- [x] **Dicionário**: `Dictionary<int, Course>` implementado para acesso rápido aos cursos via `Id`.
- [x] **Classes básicas**:
  - `Course`
  - `Applicant`
  - `TestResult`

### ✅ Lógica básica de parsing
- [x] Toda leitura e transformação dos dados de entrada está corretamente separada e modularizada (`ProcessEntrance`).
- [x] Estrutura pronta para armazenar candidatos em arrays e cursos em dicionário.

---

## ❌ Funcionalidades pendentes

### 🧮 Processamento da Seleção
- [ ] Cálculo da **média simples** das três notas para todos os candidatos.
- [ ] Aplicação do **critério de desempate** pela nota de **Redação**.
- [ ] Lógica para:
  - [ ] Selecionar candidatos nas duas opções (respeitando vagas).
  - [ ] Candidatos aprovados apenas na 2ª opção entrarem na fila da 1ª.
  - [ ] Candidatos não aprovados entrarem na fila de espera de ambos os cursos.
  - [ ] Candidatos aprovados nas duas opções serem aceitos apenas na 1ª.

### 📝 Saída no formato `saida.txt`
- [ ] Nome do curso + nota de corte (menor média entre selecionados).
- [ ] Lista de selecionados ordenada (por média, com desempate por redação).
- [ ] Lista da fila de espera, também ordenada.

### 🔢 Estrutura de Dados obrigatórias (pendentes de implementação)
- [ ] **Lista (`List<T>`)** para armazenar os candidatos **selecionados** em cada curso.
- [ ] **Fila flexível** (estrutura própria a ser criada) para armazenar a **fila de espera** de cada curso.
- [ ] Implementação de um **algoritmo de ordenação eficiente** (ex: QuickSort ou MergeSort) para as listas.

---

## 📦 Organização do Projeto
- [x] Classes separadas para entidades (`Course`, `Applicant`, `TestResult`).
- [ ] Ainda é necessário dividir o restante do código (seleção, escrita de saída, fila customizada) em classes/métodos.
