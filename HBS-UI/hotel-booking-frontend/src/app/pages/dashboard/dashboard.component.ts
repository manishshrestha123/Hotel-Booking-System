import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  hotels = [
    {
      name: 'Grand Palace Hotel',
      location: 'Kathmandu, Nepal',
      image:
        'https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?auto=format&fit=crop&w=900&q=80',
      description: 'Experience luxury in the heart of the city.',
    },
    {
      name: 'Mountain View Resort',
      location: 'Pokhara, Nepal',
      image:
        'https://images.unsplash.com/photo-1566073771259-6a8506099945?auto=format&fit=crop&w=900&q=80',
      description: 'Peaceful resort with breathtaking mountain views.',
    },
    {
      name: 'Lakeside Inn',
      location: 'Chitwan, Nepal',
      image:
        'https://images.unsplash.com/photo-1505691938895-1758d7feb511?auto=format&fit=crop&w=900&q=80',
      description: 'Relax by the lake in this cozy getaway.',
    },
  ];
}
