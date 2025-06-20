# ✅ Progresso do Projeto - Gerenciador de Processo Seletivo da Universidade Stark

# 🎓 Projeto de Simulação de Processo Seletivo

Este projeto simula um processo de admissão para cursos, com base em notas dos candidatos e suas opções de preferência. A entrada é um arquivo `.txt` com os cursos e candidatos, e a saída é outro arquivo com os resultados da seleção.

---

## 🔄 Fluxo de Execução

1. O programa lê o arquivo `entrada.txt`.
2. Os dados são interpretados e armazenados em estruturas adequadas.
3. Os candidatos são ordenados por média (e redação em caso de empate).
4. Cada candidato tenta uma vaga:
   - Primeiro na 1ª opção;
   - Se não conseguir, tenta a 2ª opção e volta para a 1ª como espera.
5. Um arquivo `saida.txt` é gerado com os resultados.

---

## 🧠 Classes e Responsabilidades

### `AdmissionManager`

- **Responsável por coordenar o processo completo**.
- Lê os dados (`ProcessEntrance`).
- Ordena os candidatos.
- Distribui entre os cursos.
- Gera o arquivo de saída.

### `Applicant`

- Representa um candidato.
- Contém:
  - Nome
  - Duas opções de curso (`Op1`, `Op2`)
  - Resultado (`TestResult`)
- Implementa `IComparable` para ordenação (por média e redação).

### `Course`

- Representa um curso com:
  - Nome, ID e número de vagas
  - Lista de aprovados (`Approvedlist`)
  - Lista de espera (`Waitlist` - do tipo `FilaFlex<Applicant>`)
- Tem lógica para:
  - Inserir candidatos (`PushNewApplicant`)
  - Verificar se ainda tem vaga (`IsApplicantAbleToApprovedlist`)

### `TestResult`

- Armazena as notas (linguagens, matemática, redação).
- Calcula automaticamente a média (`AvarageGrade`).

### `CourseOption`

- Representa uma escolha de curso feita por um candidato.
- Contém:
  - O ID do curso
  - Status da escolha (aprovado, recusado ou pendente)

### `FilaFlex<T>`

- **Fila personalizada com suporte à iteração** (`IEnumerable<T>`).
- Internamente usa a classe `Pia<T>`, que simula nós ligados.
- Métodos principais:
  - `Botar`: adiciona ao final
  - `Tirar`: remove do início
  - `ToString` e `GetEnumerator`

### `Pia<T>`

- Estrutura que representa um **nó de uma fila**.
- Tem:
  - `Tralha`: o conteúdo
  - Ponteiros para o próximo e anterior

### `FileHandler`

- Localiza o arquivo `entrada.txt` em diretórios ascendentes.
- Gera o arquivo `saida.txt` com os seguintes dados:
  - Nome do curso e nota de corte
  - Lista de aprovados com notas
  - Lista de espera com notas

### `ListExtension`

- Implementa ordenação (`QuickSort`) para listas de candidatos.
- Ordena por média (e depois por redação), em ordem **decrescente**.

---

## ✅ Regras de Seleção

- Cada candidato é avaliado por média aritmética das três notas.
- Em caso de empate, vence quem tem maior nota na redação.
- Um candidato tenta vaga no 1º curso; se não conseguir, tenta o 2º e fica na fila do 1º.
- Cada curso só aprova até o número de vagas definidas.

---
## 🔀 Ordenação de Candidatos: QuickSort

O projeto utiliza o algoritmo **QuickSort** para ordenar os candidatos antes da distribuição nos cursos. Essa ordenação é feita pela extensão `ApplicantsSort`, que aplica o `QuickSort` diretamente sobre a lista de candidatos (`List<Applicant>`), com base nos seguintes critérios:

1. **Média das notas** (ordem decrescente);
2. Em caso de empate, **nota da redação** (também decrescente).

---

### 🧠 Por que QuickSort?

A escolha do QuickSort foi baseada em três motivos principais:

#### ✅ **Eficiência**
- QuickSort tem **complexidade média de tempo `O(n log n)`**, sendo muito rápido para listas médias e grandes, como uma lista de candidatos.
- É mais eficiente que algoritmos simples como BubbleSort ou InsertionSort, especialmente com listas desordenadas.

#### ✅ **Facilidade de implementação**
- A versão utilizada no projeto é **compacta e recursiva**, implementada com comparações diretas usando `IComparable<T>`, o que a torna **fácil de adaptar** para diferentes critérios de ordenação.
- O algoritmo opera **in-place** (não precisa de memória adicional significativa).

#### ✅ **Estabilidade não é necessária**
- Como o critério de ordenação depende apenas da média e da redação, **não é necessário preservar a ordem original** dos candidatos com notas iguais.
- Por isso, a ausência de estabilidade (característica do QuickSort) **não impacta negativamente** o resultado.

---

### 🔧 Funcionamento no Projeto

A função `ApplicantsSort` é definida como uma extensão da `List<T>`, e chama internamente a versão genérica do QuickSort:

```csharp
public static void ApplicantsSort<T>(this List<T> list) where T : IComparable<T>
```
Dentro, o particionamento divide a lista com base em um pivô central, e organiza os elementos maiores primeiro (ordem decrescente), refletindo o critério de "melhor média primeiro":
```csharp
while (list[i].CompareTo(list[pivot]) > 0) i++;
while (list[j].CompareTo(list[pivot]) < 0) j--;
```
---

## 📚 FilaFlex<T>: Fila Encadeada com Iteração

### 🎯 Objetivo

A `FilaFlex<T>` foi criada como uma **implementação básica de fila encadeada**, com operações fundamentais:

- `Botar(T)`: insere no final da fila.
- `Tirar()`: remove do início da fila.
- Suporte à **iteração (`foreach`)**, através da implementação da interface `IEnumerable<T>`.

Essa estrutura foi usada para gerenciar a **fila de espera de candidatos** em cada curso, onde:

- A ordem de chegada (candidatos não selecionados inicialmente) deve ser preservada.
- Não há limite de tamanho.
- A fila precisa ser **iterável** para facilitar a geração do arquivo de saída.

---

### 🧱 Estrutura Interna

A `FilaFlex<T>` é baseada na classe auxiliar `Pia<T>`, que representa um **nó encadeado**:

- `Tralha`: conteúdo do nó.
- `Prox`: ponteiro para o próximo nó.
- `Ant`: definido, mas não usado na prática (poderia dar suporte a fila dupla).

A fila é **simplesmente encadeada**, com ponteiros para o início (_head) e o último elemento (`Last`), permitindo inserção e remoção eficientes.

---

### 🔁 Interface IEnumerable<T>

Para que a `FilaFlex<T>` possa ser usada com `foreach`, ela implementa a interface:

```csharp
public class FilaFlex<T> : IEnumerable<T>
```
O que IEnumerable<T> exige:
1. Implementar o método:
```csharp
public IEnumerator<T> GetEnumerator()
```
2. Implementar também o método não genérico:

```csharp
System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
```

### O que isso permite:
Iterar sobre a fila com foreach.

- Usar LINQ, se necessário.

- Manter o encapsulamento, sem expor internamente os nós.

### Exemplo da implementação:

```csharp
public IEnumerator<T> GetEnumerator()
{
    Pia<T>? atual = _head.Prox;
    while (atual != null)
    {
        if (atual.Tralha != null)
            yield return atual.Tralha;

        atual = atual.Prox;
    }
}
```

## 🧾 Uso no Projeto
Na geração da saída (FileHandler.GenerateOutputFile), a fila de espera de cada curso é percorrida com foreach:

```csharp
foreach (var a in course.Waitlist)
{
    sb.AppendLine($"{a.Name} {a.Result.AvarageGrade.ToString("F2").Replace('.', ',')}");
}
```
Isso só é possível porque a FilaFlex<T> implementa IEnumerable<T> corretamente.

### ✅ Benefícios da Abordagem
Simplicidade e clareza: ideal para propósitos didáticos.

- Encapsulamento: os nós (Pia<T>) não são expostos.

- Iteração natural: suporte direto a foreach.

- Flexibilidade: pode ser adaptada para outros cenários.

## 📌 Conclusão
QuickSort foi uma escolha consciente por combinar:

- Alta performance prática;

- Código simples e enxuto;

- Desnecessidade de estabilidade na ordenação.

Isso o torna ideal para ordenar candidatos em um processo seletivo, onde apenas os melhores resultados importam — independentemente da ordem original dos dados.

---

## 💡 Observações Técnicas

- O projeto evita bibliotecas externas.
- As estruturas são simples e didáticas, ideais para contextos educacionais.
---


