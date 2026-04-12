import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { RoomsRoutingModule } from './rooms-routing.module';
import { RoomsComponent } from './rooms.component';

@NgModule({
  declarations: [RoomsComponent],
  imports: [CommonModule, FormsModule, RouterModule, RoomsRoutingModule]
})
export class RoomsModule {}
