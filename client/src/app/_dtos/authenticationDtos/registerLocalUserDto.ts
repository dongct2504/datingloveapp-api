export interface RegisterLocalUserDto {
    userName: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: Date;
    gender: string;
    password: string;
    role: string | null;
}