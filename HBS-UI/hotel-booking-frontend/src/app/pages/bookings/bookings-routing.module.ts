import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookingsComponent } from './bookings.component';
import { CreateBookingComponent } from './create-booking/create-booking.component';

const routes: Routes = [
  { path: '', component: BookingsComponent },
  { path: 'create', component: CreateBookingComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BookingsRoutingModule {}
