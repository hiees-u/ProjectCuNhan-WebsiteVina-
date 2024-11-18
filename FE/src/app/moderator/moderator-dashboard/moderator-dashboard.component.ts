import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-moderator-dashboard',
  standalone: true,
  imports: [
    RouterOutlet,
  ],
  templateUrl: './moderator-dashboard.component.html',
  styleUrl: './moderator-dashboard.component.css'
})
export class ModeratorDashboardComponent {

}
