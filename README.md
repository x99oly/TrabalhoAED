# TrabalhoAED
Gerenciador do processo seletivo da Universidade Stark, com alocação por média e desempate por nota de redação.

# Requisitos

# ✅ Checklist de Requisitos - Sistema de Gerenciamento do Processo Seletivo da Universidade Stark

## 📋 Dados e Estrutura do Sistema
- [ ] Cada curso possui **número limitado de vagas**.
- [ ] Cada curso possui **fila de espera com posições infinitas**.

## 🎓 Informações do Candidato
- [ ] Cada candidato informa **duas opções de curso**.
- [ ] Cada candidato possui **nota da Redação, Matemática e Linguagens**.

## 🧮 Cálculo da Média
- [ ] A média do candidato é a **média simples** das três notas:
  - [ ] Média = (Redação + Matemática + Linguagens) / 3
- [ ] Em caso de empate na média:
  - [ ] **Desempate pela nota da Redação**
  - [ ] Não existem empates com mesma nota de Redação (sempre diferentes)

## 📊 Critérios de Seleção
- [ ] O candidato é **classificado com base na média** para cada curso.
- [ ] Se for **selecionado na 1ª opção**, ele:
  - [ ] **Não entra em nenhuma fila de espera**
- [ ] Se for **selecionado na 2ª opção**, ele:
  - [ ] É colocado na **fila de espera da 1ª opção**
- [ ] Se **não for selecionado em nenhuma das opções**, ele:
  - [ ] Entra na **fila de espera das duas opções**
- [ ] Se for **selecionado nas duas opções**, ele:
  - [ ] Entra apenas na **lista de selecionados da 1ª opção**
  - [ ] **Libera a vaga da 2ª opção**
