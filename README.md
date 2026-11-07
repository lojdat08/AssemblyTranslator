# Překladač do strojového kódu
Program na převod AVR assembly kódu do strojového kódu. (překladač)\
<br />
V překladači se dá psát assembly kód nebo jde importovat ze souboru `.asm`.
Výsledný kód lze vypsat živě v překladači nebo exportovat strojový kód do souboru `.bin` nebo `.hex` (formát Intel HEX)\
<br />

### Průběh překladu
Překlad probíhá pomocí dvou průchodů kódu. V Prvním průchodu se rozpoznávají návěstí a jejich příslušné adresy. V druhém průběhu se převedou instrukce na jejich bitové hodnoty.
<br />

## Lexikání analyzátor
Důležitou částí překladu kódu je rozdělení vstupního programu na určité části (lexémy), které jsou převedeny na symboly (tokeny).\
Ty jsou nutné na správný překlad instrukcí.

### Lexémy
Část textu, která je rozdělena jednou nebo více mezerami. Tyto části jsou následně převedeny do příslušných tokenů.
> [!NOTE]
> První lexém je vždy zkontrolovaný jestli se jedná o instrukci nebo o návěstí. Jestli se jedná o instrukci, zbytek lexémů je převeden do tokenů.

### Tokeny
Každý token má přiřazenou svojí syntaxi a svůj vlastní typ.
| Token typ | hodnoty | syntax (příklady) |
| --- | --- | --- |
| `register` | <0; 31> | r0 \| r15, \| r31 |
| `constant` | <-2ˇ31; 2ˇ31 - 1> (32 bit int) | 0 \| -230 \| 0b101 \| -0xB2 |
| `address` | <0; 2ˇ32 - 1> (32 bit unsigned int) | 0 \| 35 \| 0b1100 \| -0x5C |
| `relativeAddress` | <-2ˇ31; 2ˇ31 - 1> (32 bit int) | 0 \| -125 \| -0b0101 \| 0x2E |

### Návěstí
Návěstí je lexém, který odkazuje na danou addressu v programu.
Umožňuje tak skákat mezi částmi programu.\
Syntaxe:
Musí vždy končit znakem `:` (například `start:`)\
Pokud budeme chtít návěstí použít je nutné `:` odstranit v dané instrukci (například `JMP start`)

#### Komentáře
_Překladač podporuje možnost komentářů a to znakem `#`, cokoliv za tímto znakem je považováno za komentář a průchod programu nijak neovlivňuje._

## Podporované instrukce
| Instrukce | potřebné hodnoty (tokeny) | syntax (příklady) |
| --- | --- | --- |
| `NOP` | nic | NOP |
| `ADD` | register, register | ADD r0, r0 |
| `JMP` | address (návěstí) | JMP 0 \| JMP 0b100 \| JMP 0x1AC \| JMP start |
| `LDI` | register, constant | LDI r16 0 \| LDI r32 255 \| LDI r16 0b10 \| LDI r16 0x5A |
| `MOV` | register, register | MOV r0, r0 |
| `RJMP` | relativeAddress | RJMP 0 \| RJMP -10 \| RJMP -0b1011 \| RJMP 0xAF |
<br />

> [!WARNING]
> Některé instrukce mají hodnoty **omezené** na určitý rozsah. Je nutné si zkontrolovat rozsah dané instrukce v příslušné třídě, nebo podle [officiální dokumentace](https://ww1.microchip.com/downloads/en/DeviceDoc/AVR-Instruction-Set-Manual-DS40002198A.pdf).
<br />

## Jak sestavit projekt
> Není dokončeno, musí se dodělat
