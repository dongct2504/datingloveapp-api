export interface RegisterLocalUserDto {
    userName: string;
    email: string;
    phoneNumber: string;
    password: string;
    role: string | null;
}