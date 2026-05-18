import os
import smtplib
import sys
from email.message import EmailMessage


def main() -> int:
    host = os.environ.get("SMTP_HOST", "").strip()
    port = int(os.environ.get("SMTP_PORT", "587"))
    from_addr = os.environ.get("SMTP_FROM", "").strip()
    username = os.environ.get("SMTP_USERNAME", "").strip()
    password = "".join(os.environ.get("SMTP_PASSWORD", "").split())
    use_ssl = os.environ.get("SMTP_USE_SSL", "true").strip().lower() == "true"
    to_addr = os.environ.get("SMTP_TO", "").strip()
    subject = os.environ.get("SMTP_SUBJECT", "").strip()
    body = os.environ.get("SMTP_BODY", "")

    if not host or not from_addr or not to_addr or not subject:
        print("Missing SMTP_HOST, SMTP_FROM, SMTP_TO, or SMTP_SUBJECT.", file=sys.stderr)
        return 2

    message = EmailMessage()
    message["Subject"] = subject
    message["From"] = from_addr
    message["To"] = to_addr
    message.set_content(body)

    if use_ssl and port == 465:
        with smtplib.SMTP_SSL(host, port, timeout=20) as client:
            if username:
                client.login(username, password)
            client.send_message(message)
    else:
        with smtplib.SMTP(host, port, timeout=20) as client:
            client.ehlo()
            if use_ssl:
                client.starttls()
                client.ehlo()
            if username:
                client.login(username, password)
            client.send_message(message)

    print("SMTP_OK")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
