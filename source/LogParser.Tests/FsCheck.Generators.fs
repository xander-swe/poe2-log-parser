module FsCheckGenerators

open FsCheck
open FsCheck.FSharp

let private validUnicodeChars =
    [' ']                           // Space
    @ ['!' .. '~']                  // Basic Latin symbols                  (excluding non-printing and whitespace)
    @ ['¡' .. '¬'] @ ['®' .. 'ÿ']   // Latin-1 Supplement symbols           (excluding non-printing and whitespace)
    @ ['Ā' .. 'ſ']                  // Latin Extended-A symbols             (excluding non-printing and whitespace)
    @ ['ƀ' .. 'ɏ']                  // Latin Extended-B symbols             (excluding non-printing and whitespace)
    @ ['ɐ' .. 'ʯ']                  // IPA Extension symbols                (excluding non-printing and whitespace)
    @ ['ʰ' .. '˿']                  // Spacing Modifier Letters symbols     (excluding non-printing and whitespace) 
    @ [char 0x0300 .. char 0x036f]  // Combining Diacritical Marks symbols  (excluding non-printing and whitespace)
    @ ['Ḁ' .. 'ỿ']                  // Latin Extended Addtional symbols     (excluding non-printing and whitespace)
    @ ['ﬀ' .. 'ﬀ']                  // Latin Ligatures                      (excluding non-printing and whitespace)

let genLatinUnicodeStr minLength maxLength =
    gen {
        let! length = Gen.choose (minLength, maxLength)
        let! charList = Gen.listOfLength length (Gen.elements validUnicodeChars)
        return System.String(List.toArray charList)
    }

let latinUnicodeStr0To50Arb = genLatinUnicodeStr 0 50 |> Arb.fromGen
let latinUnicodeStr1To50Arb = genLatinUnicodeStr 1 50 |> Arb.fromGen