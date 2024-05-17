import { LocalUserDto } from "../localUserDtos/localUserDto";

export interface AuthenticationDto {
    localUserDto: LocalUserDto | null;
    token: string;
}