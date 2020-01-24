export default interface AuthenticationResponse {
    authenticated: boolean;
    firstName:     string;
    lastName:      string;
    token:         string;
    username:      string;
}