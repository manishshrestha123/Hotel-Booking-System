import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CreateRoomComponent } from './create-room.component';

const routes: Routes = [{ path: '', component: CreateRoomComponent }];

@NgModule({
  declarations: [CreateRoomComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class CreateRoomModule { }
