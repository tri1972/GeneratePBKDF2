import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EncodeComponent } from './encode/encode.component'


const routes: Routes = [
  { path: 'encode', component: EncodeComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
