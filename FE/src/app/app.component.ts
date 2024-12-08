import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, Router, NavigationEnd } from '@angular/router';
import * as AOS from 'aos'
import { filter } from 'rxjs';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Caffee VINA';
  constructor(private router: Router) {}
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    AOS.init({
      duration: 1200,  // Thời gian hiệu ứng
      once: false,      // Chạy 1 lần duy nhất khi scroll qua
      offset: 0
    });
    
    this.router.events
    .pipe(
      filter(event => event instanceof NavigationEnd)  // Chỉ lọc sự kiện NavigationEnd
    )
    .subscribe(() => {
      window.scrollTo(0, 0);  // Cuộn về đầu trang khi điều hướng
    });
  }
}
