import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthResult, LoginDto, RegisterDto } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'pizza_token';
  private readonly userKey = 'pizza_user';

  private currentUser = signal<AuthResult | null>(this.loadUser());

  readonly isLoggedIn = computed(() => !!this.currentUser());
  readonly user = computed(() => this.currentUser());
  readonly isAdmin = computed(() => this.currentUser()?.role === 'Admin');

  constructor(private http: HttpClient, private router: Router) {}

  login(dto: LoginDto) {
    return this.http.post<AuthResult>('/auth/login', dto);
  }

  register(dto: RegisterDto) {
    return this.http.post<AuthResult>('/auth/register', dto);
  }

  handleAuthSuccess(result: AuthResult) {
    localStorage.setItem(this.tokenKey, result.token);
    localStorage.setItem(this.userKey, JSON.stringify(result));
    this.currentUser.set(result);
    this.router.navigate(['/menu']);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private loadUser(): AuthResult | null {
    const data = localStorage.getItem(this.userKey);
    return data ? JSON.parse(data) : null;
  }
}
