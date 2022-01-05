import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-encode',
  templateUrl: './encode.component.html',
  styleUrls: ['./encode.component.css']
})
export class EncodeComponent implements OnInit {

  private salt: string = 'testesttest1';
  constructor() { }

  ngOnInit(): void {
    this.computeSystem_Security_CryptographyHMAC256('test@gmail.com','grteio234897uhypqa'); 
  }
async computeSystem_Security_CryptographyHMAC256(apiKey: string, apiSecret: string) {


    let password = 'testtest';
    //this.salt = 'xrgB0wr6EqfeyR4rNV3YhQ==';
    let calchash="pd0ASY6zS0tr/RqvdlXb3Tsl5vV9vjpKWPqG6tfx6LU=";

    // 文字列をTyped Arrayに変換する
    let passwordUint8Array =(new TextEncoder()).encode( window.btoa(password));
    let base64salt=window.btoa(this.salt);
    let saltUint8Array = (new TextEncoder()).encode(base64salt);
    var encodedData = window.btoa(unescape(encodeURIComponent('こんにちは')));
    /*
    let passwordUint8Array = (new TextEncoder()).encode(password);
    let saltUint8Array = (new TextEncoder()).encode(this.salt);
    // パスワードのハッシュ値を計算する。
    */
    window.crypto.subtle.importKey(
      'raw',
      passwordUint8Array,
      { name: 'PBKDF2' },
      // 鍵のエクスポートを許可するかどうかの指定。falseでエクスポートを禁止する。
      false,
      // 鍵の用途。ここでは、「鍵の変換に使う」と指定している。
      ['deriveBits']
    ).then((keyMaterial) => {
      // 乱数でsaltを作成する。
      //let salt = window.crypto.getRandomValues(new Uint8Array(16));
      window.crypto.subtle.deriveBits(
        {
          name: 'PBKDF2',
          salt: saltUint8Array,
          iterations: 10000, // ストレッチングの回数。
          hash: 'SHA-1'
        },
        keyMaterial,
        512
      ).then((buffer) => {
        var bytes = new Uint8Array(buffer);
        console.log(window.btoa(String.fromCharCode.apply(String, Array.from(bytes))));
        window.crypto

      });

    });

    /*
    // 文字列をTyped Arrayに変換する。
    let passwordUint8Array = (new TextEncoder()).encode(password);
    let saltUint8Array=(new TextEncoder()).encode(this.salt);
    // パスワードのハッシュ値を計算する。
    await window.crypto.subtle.digest(
      // ハッシュ値の計算に用いるアルゴリズム。
      { name: 'SHA-256' },
      passwordUint8Array
    ).then((digest) => {
      window.crypto.subtle.importKey(
        'raw',
        digest,
        { name: 'PBKDF2' },
        // 鍵のエクスポートを許可するかどうかの指定。falseでエクスポートを禁止する。
        false,
        // 鍵の用途。ここでは、「鍵の変換に使う」と指定している。
        ['deriveBits']
      ).then((keyMaterial) => {

        // 乱数でsaltを作成する。
        //let salt = window.crypto.getRandomValues(new Uint8Array(16));
        window.crypto.subtle.deriveBits(
          {
            name: 'PBKDF2',
            salt:saltUint8Array,
            iterations: 10000, // ストレッチングの回数。
            hash: 'SHA-512'
          },
          keyMaterial,
          512
        ).then((buffer) => {
          var bytes = new Uint8Array(buffer);
          console.log(window.btoa(String.fromCharCode.apply(String, Array.from(bytes))));

        });

      });
    });
    */
  }
}
